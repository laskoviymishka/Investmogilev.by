using System.Collections.Generic;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.Project
{
    public class InvolvedOrganization : IMongoEntity
    {
        public string OrganizationName { get; set; }
        public string ContactName { get; set; }
        public IEnumerable<AdditionalInfo> Info { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
    }
}