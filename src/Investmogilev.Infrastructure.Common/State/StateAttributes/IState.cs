// // -----------------------------------------------------------------------
// // <copyright file="IState.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.State.StateAttributes
{
	public interface IState
	{
		IStateContext Context { get; set; }
		void OnEntry();
		void OnExit();
	}
}