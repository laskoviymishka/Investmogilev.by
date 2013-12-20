using Invest.Common.State;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Invest.Common.Model.Project
{
    [BsonIgnoreExtraElements]
    public class Workflow
    {
        public List<History> History { get; set; }
        public ProjectWorkflow.State CurrentState { get; set; }
    }
}
