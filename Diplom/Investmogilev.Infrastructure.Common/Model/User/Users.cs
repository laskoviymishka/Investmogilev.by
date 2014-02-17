using System;
using Investmogilev.Infrastructure.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.User
{
	public class Users : IMongoEntity
	{
		private ObjectId _objectId;

		public string Username { get; set; }

		public string ApplicationName { get; set; }

		public DateTime CreationDate { get; set; }

		public int FailedPasswordAnswerAttemptCount { get; set; }

		public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }

		public int FailedPasswordAttemptCount { get; set; }

		public DateTime FailedPasswordAttemptWindowStart { get; set; }

		public bool IsLockedOut { get; set; }

		public DateTime LastLockoutDate { get; set; }

		public DateTime LastPasswordChangedDate { get; set; }

		public DateTime LastLoginDate { get; set; }

		public DateTime LastActivityDate { get; set; }

		public string LoweredEmail { get; set; }

		public string LoweredUsername { get; set; }

		public string Password { get; set; }

		public string PasswordQuestion { get; set; }

		public string PasswordAnswer { get; set; }

		public string Salt { get; set; }

		public string Email { get; set; }

		public bool IsApproved { get; set; }

		public string Comment { get; set; }

		public UserProfile Profile { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id
		{
			get { return _objectId.ToString(); }
			set { _objectId = ObjectId.Parse(value); }
		}
	}
}