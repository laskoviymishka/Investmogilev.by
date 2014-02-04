using Investmogilev.Infrastructure.Common.State.StateAttributes;
using Stateless;

namespace Investmogilev.Infrastructure.Common.State
{
    public interface IStateMachineBuilder
    {
        StateMachine<TS, TT> BuilStateMachine<TS,TT>(string statemachineName, IStateContext activator, TS initialState);
    }
}