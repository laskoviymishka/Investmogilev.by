using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Workflow.StateManagment
{
    public class BaseWorkflow<T> : IWorkflow where T : Invest.Common.Model.Project
    {
        #region Private Fields

        private string _currentState;
        private IList<History> _changeHistory;
        private object _context;

        #endregion

        #region Public Properties

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string CurrenState
        {
            get { return _currentState; }
            set
            {
                this.Move(_currentState, value, "", CurrentCondiotions);
            }
        }

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
            _currentState = initialState;
        }

        #endregion

        #region Methods

        public void SetContext(object context)
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

            _changeHistory.Add(new History()
            {
                EditingTime = DateTime.Now,
                FromState = from,
                ToState = to,
                Editor = editor
            });
            _currentState = transition.MoveAction.Invoke();
        }

        #endregion
    }
}