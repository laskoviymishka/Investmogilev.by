using System.Net.NetworkInformation;
using System.Runtime.Remoting.Activation;
using Invest.Common.State.StateAttributes;
using Stateless;

namespace Invest.Common.State
{
    public interface IStateMachineBuilder
    {
        StateMachine<TS, TT> BuilStateMachine<TS,TT>(string statemachineName, IStateContext activator, TS initialState);
    }
}