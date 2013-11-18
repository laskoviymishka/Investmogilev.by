using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Web.Security;

namespace Invest.Common.Model
{
    public class MongoUser : MembershipUser, MongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
    }
}
