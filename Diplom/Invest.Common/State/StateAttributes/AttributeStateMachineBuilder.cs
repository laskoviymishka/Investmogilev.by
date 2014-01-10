using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Invest.Common.Model.Common;
using Stateless;

namespace Invest.Common.State.StateAttributes
{
    public class AttributeStateMachineBuilder : IStateMachineBuilder
    {
        private static ConcurrentDictionary<Type, IState> _states;
        private IStateContext _context;

        public static void InitializeStates(Dictionary<Type, IState> container)
        {
            _states = new ConcurrentDictionary<Type, IState>(container);
        }

        public StateMachine<string, string> BuilStateMachine(string statemachineName, IStateContext context)
        {
            _context = context;
            List<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(StateAttribute)))).ToList();
            ValidateStates(types);
            ValidateTriggers(types);
            List<IState> getStates = GetStates(types);
            List<Transition> transitions = new List<Transition>();
            Transition tran = new Transition("test", "OnMap","Open","OnMap");
            tran.Guard = () => true;
            tran.From = "OnMap";
            tran.To = "Open";
            tran.Trigger = "test";
            transitions.Add(tran);
            StateMachine<string,string> machine = new StateMachine<string, string>("Open");

            foreach (IState state in getStates)
            {
                StateAttribute attribute = null;
                StateAttribute[] attrs = state.GetType().GetCustomAttributes(typeof(StateAttribute), false) as StateAttribute[];
                if (attrs.Length > 0)
                {
                    attribute = attrs[0];
                }
                var stateconfigure = machine.Configure(attribute.State).OnEntry(state.OnEntry).OnExit(state.OnExit);
                foreach (var transition in transitions.Where(t => t.From == attribute.State))
                {
                    stateconfigure.PermitIf(transition.Trigger, transition.To, transition.Guard);
                }

                foreach (var transition in transitions.Where(t => t.From == attribute.State && t.To == attribute.State))
                {
                    stateconfigure.PermitReentryIf(transition.Trigger, transition.Guard);
                }
            }

            return machine;
        }

        private void ValidateStates(List<Type> types)
        {
        }

        private void ValidateTriggers(List<Type> types)
        {
        }

        private List<IState> GetStates(List<Type> types)
        {
            List<IState> states = new List<IState>();
            foreach (var type in types)
            {
                Activator.CreateInstance(type, _context);
                states.Add(_states[type]);
            }
            return states;
        }

    }
}