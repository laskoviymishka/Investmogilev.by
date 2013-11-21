using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invest.Workflow.StateManagment
{
    public interface ITransition
    {
        string FromState { get; set; }
        string ToState { get; set; }

        Dictionary<string, Func<object, bool>> Conditions { get; set; }

        IList<Func<bool>> MoveActionFuncs { get; set; }
    }
}
