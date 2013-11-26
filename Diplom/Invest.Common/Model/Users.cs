using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Web.Security;
using MongoRepository;

namespace Invest.Common.Model
{
    public class Users : MongoEntity
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

        [BsonIgnore]
        public IEnumerable<Project> AssignProjects {
            get
            {
                List<Project> model = new List<Project>();
                model.AddRange(RepositoryContext.Current.All<GreenField>(gr => gr.AssignUser == Username));
                model.AddRange(RepositoryContext.Current.All<BrownField>(gr => gr.AssignUser == Username));
                model.AddRange(RepositoryContext.Current.All<UnUsedBuilding>(gr => gr.AssignUser == Username));
                return model;
            }
        }
    }
}
