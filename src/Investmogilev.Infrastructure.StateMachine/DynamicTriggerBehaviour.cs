// // -----------------------------------------------------------------------
// // <copyright file="DynamicTriggerBehaviour.cs" author="Andrei Tserakhau">
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
		internal class DynamicTriggerBehaviour : TriggerBehaviour
		{
			private readonly Func<object[], TState> _destination;

			public DynamicTriggerBehaviour(TTrigger trigger, Func<object[], TState> destination, Func<bool> guard)
				: base(trigger, guard)
			{
				_destination = Enforce.ArgumentNotNull(destination, "destination");
			}

			public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
			{
				destination = _destination(args);
				return true;
			}
		}
	}
}