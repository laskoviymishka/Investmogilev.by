using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.Project
{
    [BsonIgnoreExtraElements]
    public class Workflow
    {
        public History History { get; set; }
        public string CurrentState { get; set; }
    }
}
