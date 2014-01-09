using Invest.Common.State;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.Common
{
    public class MailTemplate : IMongoEntity
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public ProjectWorkflow.Trigger Trigger { get; set; }

        public UserType UserType { get; set; }
    }
}