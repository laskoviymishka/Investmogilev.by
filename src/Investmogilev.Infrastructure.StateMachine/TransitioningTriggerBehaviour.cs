// // -----------------------------------------------------------------------
// // <copyright file="TransitioningTriggerBehaviour.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.StateMachine
{
	#region Using

	using System;

	#endregion

	public partial class StateMachine<TState, TTrigger>
	{
		internal class TransitioningTriggerBehaviour : TriggerBehaviour
		{
			private readonly TState _destination;

			public TransitioningTriggerBehaviour(TTrigger trigger, TState destination, Func<bool> guard)
				: base(trigger, guard)
			{
				_destination = destination;
			}

			public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
			{
				destination = _destination;
				return true;
			}
		}
	}
}