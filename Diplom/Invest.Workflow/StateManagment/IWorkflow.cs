using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Invest.Common.Model;

namespace Invest.Workflow.StateManagment
{
    public interface IWorkflow
    {
        WorkflowEntity Workflow { get; set; }
        List<ITransition> Transitions { get; set; }
        Dictionary<string, object> CurrentCondiotions { get; set; }
        void Move(string from, string to, string editor, Dictionary<string, object> conditions);
        void SetContext(IWorkflowContext context);
    }
}