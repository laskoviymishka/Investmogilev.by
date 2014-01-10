using System.Net.NetworkInformation;
using System.Runtime.Remoting.Activation;
using Invest.Common.State.StateAttributes;
using Stateless;

namespace Invest.Common.State
{
    public interface IStateMachineBuilder
    {
        StateMachine<string, string> BuilStateMachine(string statemachineName, IStateContext activator);
    }
}