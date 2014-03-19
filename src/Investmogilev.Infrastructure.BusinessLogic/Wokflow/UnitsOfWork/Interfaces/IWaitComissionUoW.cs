// // -----------------------------------------------------------------------
// // <copyright file="IWaitComissionUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IWaitComissionUoW
	{
		void OnWaitComissionExit();

		void OnWaitComissionEntry();

		bool CouldComission();
	}
}