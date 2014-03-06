using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.User;
using Investmogilev.Infrastructure.Common.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using WebMatrix.WebData;

namespace Investmogilev.Infrastructure.BusinessLogic.Providers
{
	public class MongoMembership : ExtendedMembershipProvider
	{
		#region Private Fields

		private readonly IRepository _repository;
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

		#region Constructor

		public MongoMembership()
		{
			_repository = RepositoryContext.Current;
		}

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
			var user =
				_repository.GetOne<Users>(u => u.ApplicationName == ApplicationName && u.LoweredUsername == username.ToLower());
			if (!VerifyPassword(user, oldPassword))
			{
				return false;
			}

			var validatePasswordEventArgs = new ValidatePasswordEventArgs(username, newPassword, false);
			OnValidatingPassword(validatePasswordEventArgs);

			if (validatePasswordEventArgs.Cancel)
			{
				throw new MembershipPasswordException(validatePasswordEventArgs.FailureInformation.Message);
			}

			user.LastPasswordChangedDate = DateTime.UtcNow;
			user.Password = EncodePassword(newPassword, PasswordFormat, user.Salt);
			_repository.Update(user);
			return true;
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
			string newPasswordAnswer)
		{
			var user =
				_repository.GetOne<Users>(u => u.ApplicationName == ApplicationName && u.LoweredUsername == username.ToLower());

			if (!VerifyPassword(user, password))
			{
				return false;
			}

			user.PasswordQuestion = newPasswordQuestion;
			user.PasswordAnswer = EncodePassword(newPasswordAnswer, PasswordFormat, user.Salt);

			_repository.Update(user);
			return true;
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

			var user = new Users
			{
				Id = ObjectId.GenerateNewId().ToString(),
				ApplicationName = ApplicationName,
				CreationDate = creationDate,
				Email = email,
				FailedPasswordAnswerAttemptCount = 0,
				FailedPasswordAnswerAttemptWindowStart = creationDate,
				FailedPasswordAttemptCount = 0,
				FailedPasswordAttemptWindowStart = creationDate,
				IsApproved = isApproved,
				IsLockedOut = false,
				LastActivityDate = creationDate,
				LastLockoutDate = new DateTime(1970, 1, 1),
				LastLoginDate = creationDate,
				LastPasswordChangedDate = creationDate,
				LoweredEmail = email != null ? email.ToLowerInvariant() : null,
				LoweredUsername = username.ToLowerInvariant(),
				Password = EncodePassword(password, PasswordFormat, salt),
				PasswordAnswer = EncodePassword(passwordAnswer, PasswordFormat, salt),
				PasswordQuestion = passwordQuestion,
				Salt = salt,
				Username = username
			};
			_repository.Add(user);
			status = MembershipCreateStatus.Success;
			return GetUser(username, false);
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			try
			{
				_repository.Delete<Users>(u => u.LoweredUsername == username && u.ApplicationName == ApplicationName);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
			out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();
			emailToMatch = emailToMatch.ToLowerInvariant();
			totalRecords =
				_repository.All<Users>(
					u => u.ApplicationName == ApplicationName && u.LoweredEmail == emailToMatch).Count();

			foreach (
				Users userToConvert in
					_repository.All<Users>(
						u => u.ApplicationName == ApplicationName && u.LoweredEmail == emailToMatch)
						.Skip(pageIndex*pageSize)
						.Take(pageSize))
			{
				membershipUsers.Add(ToMembershipUser(userToConvert));
			}

			return membershipUsers;
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
			out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();
			usernameToMatch = usernameToMatch.ToLowerInvariant();
			totalRecords =
				_repository.All<Users>(
					u => u.ApplicationName == ApplicationName && u.LoweredUsername == usernameToMatch).Count();

			foreach (
				Users userToConvert in
					_repository.All<Users>(
						u => u.ApplicationName == ApplicationName && u.LoweredUsername == usernameToMatch)
						.Skip(pageIndex*pageSize)
						.Take(pageSize))
			{
				membershipUsers.Add(ToMembershipUser(userToConvert));
			}


			return membershipUsers;
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();
			totalRecords =
				_repository.All<Users>(
					u => u.ApplicationName == ApplicationName).Count();

			foreach (
				Users userToConvert in
					_repository.All<Users>(
						u => u.ApplicationName == ApplicationName)
						.Skip(pageIndex*pageSize)
						.Take(pageSize))
			{
				membershipUsers.Add(ToMembershipUser(userToConvert));
			}

			return membershipUsers;
		}

		public override int GetNumberOfUsersOnline()
		{
			TimeSpan timeSpan = TimeSpan.FromMinutes(Membership.UserIsOnlineTimeWindow);
			return _repository.All<Users>(
				u => u.ApplicationName == ApplicationName && u.LastActivityDate > DateTime.UtcNow.Subtract(timeSpan)).Count();
		}

		public override string GetPassword(string username, string answer)
		{
			username = username.ToLowerInvariant();
			if (!EnablePasswordRetrieval)
			{
				throw new NotSupportedException("This Membership Provider has not been configured to support password retrieval.");
			}

			var user = _repository.GetOne<Users>(u => u.ApplicationName == ApplicationName && u.LoweredUsername == username);

			if (RequiresQuestionAndAnswer && !VerifyPasswordAnswer(user, answer))
			{
				throw new MembershipPasswordException("The password-answer supplied is invalid.");
			}

			return DecodePassword(user.Password, PasswordFormat);
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			username = username.ToLowerInvariant();
			var user =
				_repository.GetOne<Users>(
					u => u.LoweredUsername == username && u.ApplicationName == ApplicationName);
			if (user == null)
			{
				return null;
			}

			if (userIsOnline)
			{
				user.LastActivityDate = DateTime.UtcNow;
				_repository.Update(user);
			}

			return ToMembershipUser(user);
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			var stringUserKey = providerUserKey.ToString();
			var user = _repository.GetOne<Users>(u => u.Id == stringUserKey);

			if (user == null)
			{
				return null;
			}

			if (userIsOnline)
			{
				user.LastActivityDate = DateTime.UtcNow;
				_repository.Update(user);
			}

			return ToMembershipUser(user);
		}

		private static ObjectId GenerateObjectId(object providerUserKey)
		{
			byte[] bytes = ((Guid) providerUserKey).ToByteArray().Take(12).ToArray();
			var oid = new ObjectId(bytes);
			return oid;
		}

		public override string GetUserNameByEmail(string email)
		{
			email = email.ToLowerInvariant();
			if (string.IsNullOrWhiteSpace(email))
			{
				return null;
			}
			var user = _repository.GetOne<Users>(u => u.LoweredEmail == email);
			return user != null ? user.Username : null;
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
			username = username.ToLowerInvariant();
			if (!EnablePasswordReset)
			{
				throw new NotSupportedException(
					"This provider is not configured to allow password resets. To enable password reset, set enablePasswordReset to \"true\" in the configuration file.");
			}
			var user = _repository.GetOne<Users>(u => u.LoweredUsername == username);

			if (RequiresQuestionAndAnswer && !VerifyPasswordAnswer(user, answer))
			{
				throw new MembershipPasswordException("The password-answer supplied is invalid.");
			}

			string password = Membership.GeneratePassword(MinRequiredPasswordLength, MinRequiredNonAlphanumericCharacters);

			user.LastPasswordChangedDate = DateTime.UtcNow;
			user.Password = EncodePassword(password, PasswordFormat, user.Salt);
			_repository.Update(user);
			return password;
		}

		public override bool UnlockUser(string username)
		{
			username = username.ToLowerInvariant();
			var user = _repository.GetOne<Users>(u => u.LoweredUsername == username && u.ApplicationName == ApplicationName);
			user.FailedPasswordAnswerAttemptCount = 0;
			user.FailedPasswordAttemptCount = 0;
			user.FailedPasswordAnswerAttemptWindowStart = new DateTime(1970, 1, 1);
			user.FailedPasswordAttemptWindowStart = new DateTime(1970, 1, 1);
			user.IsLockedOut = false;
			user.LastLockoutDate = new DateTime(1970, 1, 1);
			_repository.Update(user);
			return true;
		}

		public override void UpdateUser(MembershipUser user)
		{
			var userToUpdate = _repository.GetOne<Users>(u => u.Id == user.ProviderUserKey.ToString());
			if (userToUpdate == null)
			{
				throw new ProviderException("The user was not found.");
			}
			userToUpdate.Comment = user.Comment;
			userToUpdate.Email = user.Email;
			userToUpdate.IsApproved = user.IsApproved;
			userToUpdate.LastActivityDate = user.LastActivityDate;
			userToUpdate.LastLoginDate = user.LastLoginDate;
			userToUpdate.LoweredEmail = user.Email.ToLowerInvariant();
			_repository.Update(userToUpdate);
		}

		public override bool ValidateUser(string username, string password)
		{
			username = username.ToLowerInvariant();
			var user = _repository.GetOne<Users>(u => u.LoweredUsername == username && u.ApplicationName == ApplicationName);

			if (user == null || !user.IsApproved || user.IsLockedOut)
			{
				return false;
			}

			if (VerifyPassword(user, password))
			{
				user.LastLoginDate = DateTime.UtcNow;
				_repository.Update(user);
				return true;
			}
			user.FailedPasswordAttemptCount++;
			user.FailedPasswordAttemptWindowStart = DateTime.UtcNow;
			_repository.Update(user);
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

		private MembershipUser ToMembershipUser(Users userToConvert)
		{
			if (userToConvert == null)
			{
				return null;
			}

			string email = userToConvert.Email;
			return new MembershipUser(Name,
				userToConvert.Username,
				userToConvert.Id,
				email,
				userToConvert.PasswordQuestion,
				userToConvert.Comment,
				userToConvert.IsApproved,
				userToConvert.IsLockedOut,
				userToConvert.CreationDate,
				userToConvert.LastLoginDate,
				userToConvert.LastActivityDate,
				userToConvert.LastPasswordChangedDate,
				userToConvert.LastLockoutDate);
		}

		private bool VerifyPassword(Users user, string password)
		{
			return user.Password == EncodePassword(password, PasswordFormat, user.Salt);
		}

		private bool VerifyPasswordAnswer(Users user, string passwordAnswer)
		{
			return user.PasswordAnswer == EncodePassword(passwordAnswer, PasswordFormat, user.Salt);
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