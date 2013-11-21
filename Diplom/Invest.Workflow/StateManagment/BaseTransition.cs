using System;
using System.Collections.Generic;

namespace Invest.Workflow.StateManagment
{
    public class BaseTransition : ITransition
    {
        public BaseTransition(string from, string to, Dictionary<string, Func<object, bool>> conditions, IList<Func<bool>> moveAction)
        {
            FromState = from;
            ToState = to;
            Conditions = conditions;
            MoveActionFuncs = moveAction;
        }

        public string FromState
        {
            get;
            set;
        }

        public string ToState
        {
            get;
            set;
        }

        public Dictionary<string, Func<object, bool>> Conditions
        {
            get;
            set;
        }

        public IList<Func<bool>> MoveActionFuncs
        {
            get;
            set;
        }
    }
}