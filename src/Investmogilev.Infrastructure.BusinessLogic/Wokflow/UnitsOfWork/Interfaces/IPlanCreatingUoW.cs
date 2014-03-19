// // -----------------------------------------------------------------------
// // <copyright file="IPlanCreatingUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IPlanCreatingUoW
	{
		void OnPlanCreatingExit();

		void OnPlanCreatingEntry();

		bool CouldUpdatePlan();

		bool CouldApprovePlan();
	}
}