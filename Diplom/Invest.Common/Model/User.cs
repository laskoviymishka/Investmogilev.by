using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Web.Security;

namespace Invest.Common.Model
{
    public class MongoUser : MembershipUser, MongoEntity
    {
        private ObjectId _objectId;

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id
        {
            get { return _objectId.ToString(); }
            set { _objectId = ObjectId.Parse(value); }
        }

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

        public string LoweredEmail { get; set; }

        public string LoweredUsername { get; set; }

        public string Password { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public string Salt { get; set; }
    }
}
