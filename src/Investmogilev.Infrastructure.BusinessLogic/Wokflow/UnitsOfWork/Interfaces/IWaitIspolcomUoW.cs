// // -----------------------------------------------------------------------
// // <copyright file="IWaitIspolcomUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IWaitIspolcomUoW
	{
		void OnWaitIspolcomExit();

		void OnWaitIspolcomEntry();

		bool CouldToIspolcom();
	}
}