// // -----------------------------------------------------------------------
// // <copyright file="BaseState.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.State.StateAttributes
{
	public abstract class BaseState : IState
	{
		protected BaseState(IStateContext context)
		{
			Context = context;
		}

		public abstract void OnEntry();

		public abstract void OnExit();

		public IStateContext Context { get; set; }
	}
}