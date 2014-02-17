using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Investmogilev.Infrastructure.Common.Model.User;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using WebMatrix.WebData;

namespace Investmogilev.Infrastructure.BusinessLogic.Providers
{
	public class MongoMembership : ExtendedMembershipProvider
	{
		#region Private Fields

		private bool enablePasswordReset;
		private bool enablePasswordRetrieval;
		private int maxInvalidPasswordAttempts;
		private int minRequiredNonAlphanumericCharacters;
		private int minRequiredPasswordLength;
		private MongoCollection mongoCollection;
		private int passwordAttemptWindow;
		private MembershipPasswordFormat passwordFormat;
		private string passwordStrengthRegularExpression;
		private bool requiresQuestionAndAnswer;
		private bool requiresUniqueEmail;

		#endregion

		#region Public Properties

		public override string ApplicationName { get; set; }

		public override bool EnablePasswordReset
		{
			get { return enablePasswordReset; }
		}

		public override bool EnablePasswordRetrieval
		{
			get { return enablePasswordRetrieval; }
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { return maxInvalidPasswordAttempts; }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return minRequiredNonAlphanumericCharacters; }
		}

		public override int MinRequiredPasswordLength
		{
			get { return minRequiredPasswordLength; }
		}

		public override int PasswordAttemptWindow
		{
			get { return passwordAttemptWindow; }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { return passwordFormat; }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { return passwordStrengthRegularExpression; }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { return requiresQuestionAndAnswer; }
		}

		public override bool RequiresUniqueEmail
		{
			get { return requiresUniqueEmail; }
		}

		#endregion

		#region Membership methods

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.EQ("LoweredUsername", username.ToLowerInvariant()));
			var bsonDocument = mongoCollection.FindOneAs<BsonDocument>(query);

			if (!VerifyPassword(bsonDocument, oldPassword))
			{
				return false;
			}

			var validatePasswordEventArgs = new ValidatePasswordEventArgs(username, newPassword, false);
			OnValidatingPassword(validatePasswordEventArgs);

			if (validatePasswordEventArgs.Cancel)
			{
				throw new MembershipPasswordException(validatePasswordEventArgs.FailureInformation.Message);
			}

			UpdateBuilder update = Update.Set("LastPasswordChangedDate", DateTime.UtcNow)
				.Set("Password", EncodePassword(newPassword, PasswordFormat, bsonDocument["Salt"].AsString));
			mongoCollection.Update(query, update);

			return true;
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
			string newPasswordAnswer)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.EQ("LoweredUsername", username.ToLowerInvariant()));
			var bsonDocument = mongoCollection.FindOneAs<BsonDocument>(query);

			if (!VerifyPassword(bsonDocument, password))
			{
				return false;
			}

			UpdateBuilder update = Update.Set("PasswordQuestion", newPasswordQuestion)
				.Set("PasswordAnswer", EncodePassword(newPasswordAnswer, PasswordFormat, bsonDocument["Salt"].AsString));
			return mongoCollection.Update(query, update).Ok;
		}

		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
			string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			if (providerUserKey != null && !(providerUserKey is Guid))
			{
				status = MembershipCreateStatus.InvalidProviderUserKey;
				return null;
			}
			providerUserKey = Guid.NewGuid();

			var validatePasswordEventArgs = new ValidatePasswordEventArgs(username, password, true);
			OnValidatingPassword(validatePasswordEventArgs);

			if (validatePasswordEventArgs.Cancel)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}

			if (RequiresQuestionAndAnswer && string.IsNullOrWhiteSpace(passwordQuestion))
			{
				status = MembershipCreateStatus.InvalidQuestion;
				return null;
			}

			if (RequiresQuestionAndAnswer && string.IsNullOrWhiteSpace(passwordAnswer))
			{
				status = MembershipCreateStatus.InvalidAnswer;
				return null;
			}

			if (GetUser(username, false) != null)
			{
				status = MembershipCreateStatus.DuplicateUserName;
				return null;
			}

			if (GetUser(providerUserKey, false) != null)
			{
				status = MembershipCreateStatus.DuplicateProviderUserKey;
				return null;
			}

			if (RequiresUniqueEmail && !string.IsNullOrWhiteSpace(GetUserNameByEmail(email)))
			{
				status = MembershipCreateStatus.DuplicateEmail;
				return null;
			}

			var buffer = new byte[16];
			(new RNGCryptoServiceProvider()).GetBytes(buffer);
			string salt = Convert.ToBase64String(buffer);

			DateTime creationDate = DateTime.UtcNow;

			var bsonDocument = new BsonDocument
			{
				{"Id", new ObjectId()},
				{"ApplicationName", ApplicationName},
				{"CreationDate", creationDate},
				{"Email", email},
				{"FailedPasswordAnswerAttemptCount", 0},
				{"FailedPasswordAnswerAttemptWindowStart", creationDate},
				{"FailedPasswordAttemptCount", 0},
				{"FailedPasswordAttemptWindowStart", creationDate},
				{"IsApproved", isApproved},
				{"IsLockedOut", false},
				{"LastActivityDate", creationDate},
				{"LastLockoutDate", new DateTime(1970, 1, 1)},
				{"LastLoginDate", creationDate},
				{"LastPasswordChangedDate", creationDate},
				{"LoweredEmail", email != null ? email.ToLowerInvariant() : null},
				{"LoweredUsername", username.ToLowerInvariant()},
				{"Password", EncodePassword(password, PasswordFormat, salt)},
				{"PasswordAnswer", EncodePassword(passwordAnswer, PasswordFormat, salt)},
				{"PasswordQuestion", passwordQuestion},
				{"Salt", salt},
				{"Username", username}
			};

			mongoCollection.Insert(bsonDocument);
			status = MembershipCreateStatus.Success;
			return GetUser(username, false);
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.EQ("LoweredUsername", username.ToLowerInvariant()));

			WriteConcernResult writeConcernResult = mongoCollection.Remove(query, WriteConcern.Acknowledged);

			if (writeConcernResult != null)
			{
				return writeConcernResult.DocumentsAffected >= 1;
			}

			return false;
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
			out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();

			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.Matches("LoweredEmail", emailToMatch.ToLowerInvariant()));
			totalRecords = (int) mongoCollection.FindAs<BsonDocument>(query).Count();

			foreach (
				BsonDocument bsonDocument in
					mongoCollection.FindAs<BsonDocument>(query).SetSkip(pageIndex*pageSize).SetLimit(pageSize))
			{
				membershipUsers.Add(ToMembershipUser(bsonDocument));
			}

			return membershipUsers;
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
			out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();

			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.Matches("LoweredUsername", usernameToMatch.ToLowerInvariant()));
			totalRecords = (int) mongoCollection.FindAs<BsonDocument>(query).Count();

			foreach (
				BsonDocument bsonDocument in
					mongoCollection.FindAs<BsonDocument>(query).SetSkip(pageIndex*pageSize).SetLimit(pageSize))
			{
				membershipUsers.Add(ToMembershipUser(bsonDocument));
			}

			return membershipUsers;
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();

			IMongoQuery query = Query.EQ("ApplicationName", ApplicationName);
			totalRecords = (int) mongoCollection.FindAs<BsonDocument>(query).Count();

			foreach (
				BsonDocument bsonDocument in
					mongoCollection.FindAs<BsonDocument>(query).SetSkip(pageIndex*pageSize).SetLimit(pageSize))
			{
				membershipUsers.Add(ToMembershipUser(bsonDocument));
			}

			return membershipUsers;
		}

		public override int GetNumberOfUsersOnline()
		{
			TimeSpan timeSpan = TimeSpan.FromMinutes(Membership.UserIsOnlineTimeWindow);
			return
				(int)
					mongoCollection.Count(Query.And(Query.EQ("ApplicationName", ApplicationName),
						Query.GT("LastActivityDate", DateTime.UtcNow.Subtract(timeSpan))));
		}

		public override string GetPassword(string username, string answer)
		{
			if (!EnablePasswordRetrieval)
			{
				throw new NotSupportedException("This Membership Provider has not been configured to support password retrieval.");
			}

			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.EQ("LoweredUsername", username.ToLowerInvariant()));
			var bsonDocument = mongoCollection.FindOneAs<BsonDocument>(query);

			if (RequiresQuestionAndAnswer && !VerifyPasswordAnswer(bsonDocument, answer))
			{
				throw new MembershipPasswordException("The password-answer supplied is invalid.");
			}

			return DecodePassword(bsonDocument["Password"].AsString, PasswordFormat);
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.EQ("LoweredUsername", username.ToLowerInvariant()));
			var bsonDocument = mongoCollection.FindOneAs<BsonDocument>(query);

			if (bsonDocument == null)
			{
				return null;
			}

			if (userIsOnline)
			{
				UpdateBuilder update = Update.Set("LastActivityDate", DateTime.UtcNow);
				mongoCollection.Update(query, update);
			}

			return ToMembershipUser(bsonDocument);
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			IMongoQuery query = Query.EQ("Id", GenerateObjectId(providerUserKey));
			var bsonDocument = mongoCollection.FindOneAs<BsonDocument>(query);

			if (bsonDocument == null)
			{
				return null;
			}

			if (userIsOnline)
			{
				UpdateBuilder update = Update.Set("LastActivityDate", DateTime.UtcNow);
				mongoCollection.Update(query, update);
			}

			return ToMembershipUser(bsonDocument);
		}

		private static ObjectId GenerateObjectId(object providerUserKey)
		{
			byte[] bytes = ((Guid) providerUserKey).ToByteArray().Take(12).ToArray();
			var oid = new ObjectId(bytes);
			return oid;
		}

		public override string GetUserNameByEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return null;
			}

			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.EQ("LoweredEmail", email.ToLowerInvariant()));
			var bsonDocument = mongoCollection.FindOneAs<BsonDocument>(query);
			return bsonDocument != null ? bsonDocument["Username"].AsString : null;
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			ApplicationName = config["applicationName"] ?? "Default";
			enablePasswordReset = bool.Parse(config["enablePasswordReset"] ?? "true");
			enablePasswordRetrieval = bool.Parse(config["enablePasswordRetrieval"] ?? "false");
			maxInvalidPasswordAttempts = int.Parse(config["maxInvalidPasswordAttempts"] ?? "5");
			minRequiredNonAlphanumericCharacters = int.Parse(config["minRequiredNonAlphanumericCharacters"] ?? "1");
			minRequiredPasswordLength = int.Parse(config["minRequiredPasswordLength"] ?? "7");
			passwordAttemptWindow = int.Parse(config["passwordAttemptWindow"] ?? "10");
			passwordFormat =
				(MembershipPasswordFormat) Enum.Parse(typeof (MembershipPasswordFormat), config["passwordFormat"] ?? "Hashed");
			passwordStrengthRegularExpression = config["passwordStrengthRegularExpression"] ?? string.Empty;
			requiresQuestionAndAnswer = bool.Parse(config["requiresQuestionAndAnswer"] ?? "false");
			requiresUniqueEmail = bool.Parse(config["requiresUniqueEmail"] ?? "true");

			if (PasswordFormat == MembershipPasswordFormat.Hashed && EnablePasswordRetrieval)
			{
				throw new ProviderException(
					"Configured settings are invalid: Hashed passwords cannot be retrieved. Either set the password format to different type, or set enablePasswordRetrieval to false.");
			}

			mongoCollection =
				new MongoClient(config["connectionString"] ?? "mongodb://localhost").GetServer()
					.GetDatabase(config["database"] ?? "ASPNETDB")
					.GetCollection(config["collection"] ?? "Users");
			mongoCollection.EnsureIndex("ApplicationName");
			mongoCollection.EnsureIndex("ApplicationName", "LoweredEmail");
			mongoCollection.EnsureIndex("ApplicationName", "LoweredUsername");

			base.Initialize(name, config);
		}

		public override string ResetPassword(string username, string answer)
		{
			if (!EnablePasswordReset)
			{
				throw new NotSupportedException(
					"This provider is not configured to allow password resets. To enable password reset, set enablePasswordReset to \"true\" in the configuration file.");
			}

			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.EQ("LoweredUsername", username.ToLowerInvariant()));
			var bsonDocument = mongoCollection.FindOneAs<BsonDocument>(query);

			if (RequiresQuestionAndAnswer && !VerifyPasswordAnswer(bsonDocument, answer))
			{
				throw new MembershipPasswordException("The password-answer supplied is invalid.");
			}

			string password = Membership.GeneratePassword(MinRequiredPasswordLength, MinRequiredNonAlphanumericCharacters);
			UpdateBuilder update = Update.Set("LastPasswordChangedDate", DateTime.UtcNow)
				.Set("Password", EncodePassword(password, PasswordFormat, bsonDocument["Salt"].AsString));
			mongoCollection.Update(query, update);

			return password;
		}

		public override bool UnlockUser(string username)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.EQ("LoweredUsername", username.ToLowerInvariant()));
			UpdateBuilder update =
				Update.Set("FailedPasswordAttemptCount", 0)
					.Set("FailedPasswordAttemptWindowStart", new DateTime(1970, 1, 1))
					.Set("FailedPasswordAnswerAttemptCount", 0)
					.Set("FailedPasswordAnswerAttemptWindowStart", new DateTime(1970, 1, 1))
					.Set("IsLockedOut", false)
					.Set("LastLockoutDate", new DateTime(1970, 1, 1));
			return mongoCollection.Update(query, update).Ok;
		}

		public override void UpdateUser(MembershipUser user)
		{
			IMongoQuery query = Query.EQ("Id", GenerateObjectId(user.ProviderUserKey));
			var bsonDocument = mongoCollection.FindOneAs<BsonDocument>(query);

			if (bsonDocument == null)
			{
				throw new ProviderException("The user was not found.");
			}

			UpdateBuilder update = Update.Set("ApplicationName", ApplicationName)
				.Set("Comment", user.Comment)
				.Set("Email", user.Email)
				.Set("IsApproved", user.IsApproved)
				.Set("LastActivityDate", user.LastActivityDate.ToUniversalTime())
				.Set("LastLoginDate", user.LastLoginDate.ToUniversalTime())
				.Set("LoweredEmail", user.Email.ToLowerInvariant());

			mongoCollection.Update(query, update);
		}

		public override bool ValidateUser(string username, string password)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.EQ("LoweredUsername", username.ToLowerInvariant()));
			var bsonDocument = mongoCollection.FindOneAs<BsonDocument>(query);

			if (bsonDocument == null || !bsonDocument["IsApproved"].AsBoolean || bsonDocument["IsLockedOut"].AsBoolean)
			{
				return false;
			}

			if (VerifyPassword(bsonDocument, password))
			{
				mongoCollection.Update(query, Update.Set("LastLoginDate", DateTime.UtcNow));
				return true;
			}

			mongoCollection.Update(query,
				Update.Inc("FailedPasswordAttemptCount", 1).Set("FailedPasswordAttemptWindowStart", DateTime.UtcNow));
			return false;
		}

		#region Private Methods

		private string DecodePassword(string password, MembershipPasswordFormat membershipPasswordFormat)
		{
			switch (passwordFormat)
			{
				case MembershipPasswordFormat.Clear:
					return password;

				case MembershipPasswordFormat.Hashed:
					throw new ProviderException("Hashed passwords cannot be decoded.");

				default:
					byte[] passwordBytes = Convert.FromBase64String(password);
					byte[] decryptedBytes = DecryptPassword(passwordBytes);
					return decryptedBytes == null ? null : Encoding.Unicode.GetString(decryptedBytes, 16, decryptedBytes.Length - 16);
			}
		}

		private string EncodePassword(string password, MembershipPasswordFormat membershipPasswordFormat, string salt)
		{
			if (password == null)
			{
				return null;
			}

			if (membershipPasswordFormat == MembershipPasswordFormat.Clear)
			{
				return password;
			}

			byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
			byte[] saltBytes = Convert.FromBase64String(salt);
			var allBytes = new byte[saltBytes.Length + passwordBytes.Length];

			Buffer.BlockCopy(saltBytes, 0, allBytes, 0, saltBytes.Length);
			Buffer.BlockCopy(passwordBytes, 0, allBytes, saltBytes.Length, passwordBytes.Length);

			if (membershipPasswordFormat == MembershipPasswordFormat.Hashed)
			{
				return Convert.ToBase64String(HashAlgorithm.Create("SHA1").ComputeHash(allBytes));
			}

			return Convert.ToBase64String(EncryptPassword(allBytes));
		}

		private MembershipUser ToMembershipUser(BsonDocument bsonDocument)
		{
			if (bsonDocument == null)
			{
				return null;
			}

			string email = bsonDocument.Contains("Email") ? bsonDocument["Email"].AsString : null;
			string passwordQuestion = "";
			var user = new Users();
			return new MembershipUser(Name,
				bsonDocument["Username"].AsString,
				bsonDocument["Id"].AsObjectId,
				email,
				passwordQuestion,
				"",
				bsonDocument["IsApproved"].AsBoolean,
				bsonDocument["IsLockedOut"].AsBoolean,
				bsonDocument["CreationDate"].ToUniversalTime(),
				bsonDocument["LastLoginDate"].ToUniversalTime(),
				bsonDocument["LastActivityDate"].ToUniversalTime(),
				bsonDocument["LastPasswordChangedDate"].ToUniversalTime(),
				bsonDocument["LastLockoutDate"].ToUniversalTime());
		}

		private bool VerifyPassword(BsonDocument user, string password)
		{
			return user["Password"].AsString == EncodePassword(password, PasswordFormat, user["Salt"].AsString);
		}

		private bool VerifyPasswordAnswer(BsonDocument user, string passwordAnswer)
		{
			return user["PasswordAnswer"].AsString == EncodePassword(passwordAnswer, PasswordFormat, user["Salt"].AsString);
		}

		#endregion

		#endregion

		#region ExtendedMembership Methods

		public override bool ConfirmAccount(string accountConfirmationToken)
		{
			throw new NotImplementedException();
		}

		public override bool ConfirmAccount(string userName, string accountConfirmationToken)
		{
			throw new NotImplementedException();
		}

		public override string CreateAccount(string userName, string password, bool requireConfirmationToken)
		{
			MembershipCreateStatus status;
			CreateUser(userName, password, "", "", "", true, new Guid(), out status);
			return status.ToString();
		}

		public override string CreateUserAndAccount(string userName, string password, bool requireConfirmation,
			IDictionary<string, object> values)
		{
			return CreateAccount(userName, password);
		}

		public override bool DeleteAccount(string userName)
		{
			throw new NotImplementedException();
		}

		public override string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow)
		{
			throw new NotImplementedException();
		}

		public override ICollection<OAuthAccountData> GetAccountsForUser(string userName)
		{
			throw new NotImplementedException();
		}

		public override DateTime GetCreateDate(string userName)
		{
			throw new NotImplementedException();
		}

		public override DateTime GetLastPasswordFailureDate(string userName)
		{
			throw new NotImplementedException();
		}

		public override DateTime GetPasswordChangedDate(string userName)
		{
			throw new NotImplementedException();
		}

		public override int GetPasswordFailuresSinceLastSuccess(string userName)
		{
			throw new NotImplementedException();
		}

		public override int GetUserIdFromPasswordResetToken(string token)
		{
			throw new NotImplementedException();
		}

		public override bool IsConfirmed(string userName)
		{
			throw new NotImplementedException();
		}

		public override bool ResetPasswordWithToken(string token, string newPassword)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}