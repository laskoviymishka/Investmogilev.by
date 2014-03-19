// // -----------------------------------------------------------------------
// // <copyright file="StateReference.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.StateMachine
{
	public partial class StateMachine<TState, TTrigger>
	{
		internal class StateReference
		{
			public TState State { get; set; }
		}
	}
}