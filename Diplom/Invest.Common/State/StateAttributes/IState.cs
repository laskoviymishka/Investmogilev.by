using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invest.Common.State.StateAttributes
{
    public interface IState
    {
        IStateContext Context { get; set; }
        void OnEntry();
        void OnExit();
    }
}
