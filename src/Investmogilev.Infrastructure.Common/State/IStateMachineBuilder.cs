// // -----------------------------------------------------------------------
// // <copyright file="IStateMachineBuilder.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.State
{
	#region Using

	using Investmogilev.Infrastructure.Common.State.StateAttributes;
	using Investmogilev.Infrastructure.StateMachine;

	#endregion

	public interface IStateMachineBuilder
	{
		StateMachine<TS, TT> BuilStateMachine<TS, TT>(string statemachineName, IStateContext activator, TS initialState);
	}
}