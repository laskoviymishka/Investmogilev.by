using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Web.Security;

namespace Invest.Common.Model
{
    public class User : MembershipUser
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
    }
}
