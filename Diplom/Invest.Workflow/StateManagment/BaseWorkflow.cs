using System;
using System.Collections.Generic;
using Invest.Common.Model;
using Invest.Common.Model.ProjectModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Workflow.StateManagment
{
    public class BaseWorkflow<T> : IWorkflow
    {
        #region Private Fields

        private IList<History> _changeHistory;
        private IWorkflowContext _context;

        #endregion

        #region Public Properties

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string CurrenState
        {
            get { return Workflow.CurrenState.ToString(); }
            set
            {
                Move(Workflow.CurrenState.ToString(), value, "", CurrentCondiotions);
            }
        }

        public WorkflowEntity Workflow { get; set; }

        public Dictionary<string, object> CurrentCondiotions { get; set; }

        public List<ITransition> Transitions { get; set; }

        public IList<History> ChangeHistory
        {
            get { return _changeHistory; }
            set { _changeHistory = value; }
        }

        #endregion

        #region Constructor

        public BaseWorkflow(string initialState)
        {
            if (Workflow == null)
            {
                Workflow = new WorkflowEntity();
            }
        }

        #endregion

        #region Methods

        public void SetContext(IWorkflowContext context)
        {
            _context = context;
        }

        public void Move(string from, string to, string editor, Dictionary<string, object> conditions)
        {
            var transition = Transitions.Find(t => t.FromState == from && t.ToState == to);
            foreach (var conditionKey in transition.Conditions.Keys)
            {
                if (conditions.ContainsKey(conditionKey))
                {
                    if (!(transition.Conditions[conditionKey].Invoke(CurrentCondiotions[conditionKey])))
                    {
                        throw new Exception("Failt condition");
                    }
                }
                else
                {
                    throw new Exception("Missed condition");
                }
            }

          
            _context.SaveState(this);
        }

        #endregion
    }
}