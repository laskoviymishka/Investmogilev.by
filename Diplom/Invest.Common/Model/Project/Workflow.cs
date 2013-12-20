using Invest.Common.State;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.Project
{
    [BsonIgnoreExtraElements]
    public class Workflow
    {
        public History History { get; set; }
        public ProjectWorkflow.State CurrentState { get; set; }
    }
}
