using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model
{
    public class WorkflowEntity : MongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string ProjectId { get; set; }
        public string CurrenState { get; set; }
        public IList<History> ChangeHistory { get; set; }


        public override string ToString()
        {
            return CurrenState;
        }
    }
}