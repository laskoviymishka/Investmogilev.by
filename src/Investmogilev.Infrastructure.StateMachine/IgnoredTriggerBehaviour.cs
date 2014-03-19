// // -----------------------------------------------------------------------
// // <copyright file="IgnoredTriggerBehaviour.cs" author="Andrei Tserakhau">
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
		internal class IgnoredTriggerBehaviour : TriggerBehaviour
		{
			public IgnoredTriggerBehaviour(TTrigger trigger, Func<bool> guard)
				: base(trigger, guard)
			{
			}

			public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
			{
				destination = default(TState);
				return false;
			}
		}
	}
}