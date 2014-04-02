// // -----------------------------------------------------------------------
// // <copyright file="IRealizationUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IRealizationUoW
	{
		void OnRealizationExit();

		void OnRealizationEntry();

		bool CouldUpdateRealization();

		bool CouldUpdateRealizationAndLeave();
	}
}