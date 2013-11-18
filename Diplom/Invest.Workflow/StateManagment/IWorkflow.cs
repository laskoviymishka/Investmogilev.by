using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Workflow.StateManagment
{
    public interface IWorkflow
    {
        [BsonRepresentation(BsonType.ObjectId)]
        string _id { get; set; }
        string CurrenState { get; set; }
        Dictionary<string, object> CurrentCondiotions { get; set; }
        List<ITransition> Transitions { get; set; }
        void Move(string from, string to, string editor, Dictionary<string, object> conditions);
        IList<History> ChangeHistory { get; set; }
        void SetContext(object context);
    }
}