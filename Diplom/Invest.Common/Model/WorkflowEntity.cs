using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model
{
    public class WorkflowEntity 
    {
        public string CurrenState { get; set; }
        public IList<History> ChangeHistory { get; set; }


        public override string ToString()
        {
            return CurrenState;
        }
    }
}