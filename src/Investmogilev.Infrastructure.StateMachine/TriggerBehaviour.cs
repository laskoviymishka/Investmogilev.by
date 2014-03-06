using System;

namespace Investmogilev.Infrastructure.StateMachine
{
	public partial class StateMachine<TState, TTrigger>
	{
		internal abstract class TriggerBehaviour
		{
			private readonly Func<bool> _guard;
			private readonly TTrigger _trigger;

			protected TriggerBehaviour(TTrigger trigger, Func<bool> guard)
			{
				_trigger = trigger;
				_guard = guard;
			}

			public TTrigger Trigger
			{
				get { return _trigger; }
			}

			public bool IsGuardConditionMet
			{
				get { return _guard(); }
			}

			public abstract bool ResultsInTransitionFrom(TState source, object[] args, out TState destination);
		}
	}
}