using System;
using System.Collections.Generic;
using System.Linq;

namespace Invest.Common.State.StateAttributes
{
	public class StateMachine : IStateMachine<string>
	{
		internal StateMachine(IEnumerable<Transition> transitions, TS initialState)
		{
			Transitions = transitions;
			CurrentState = initialState;
		}

		public TS CurrentState { get; private set; }
		public IEnumerable<Transition> Transitions { get; private set; }

		public void Move(object trigger)
		{
			foreach (var transition in Transitions.Where(t => t.Trigger.ToString() == trigger))
			{
				if (transition.From == CurrentState)
				{
					if (transition.Guard.Invoke())
					{
						CurrentState = (TS)transition.To;
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
			}
		}

		public bool TryFireTrigger(string trigger)
		{
			foreach (var transition in Transitions.Where(t => t.Trigger.ToString() == trigger))
			{
				if (transition.From == CurrentState)
				{
					if (transition.Guard.Invoke())
					{
						CurrentState = transition.To.ToString();
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			return false;
		}

		public bool CanFire(string trigger)
		{
			foreach (var transition in Transitions.Where(t => t.Trigger.ToString() == trigger))
			{
				if (transition.From == CurrentState)
				{
					if (transition.Guard.Invoke())
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			return false;
		}

		public string GetState
		{
			get { throw new NotImplementedException(); }
		}
	}
}