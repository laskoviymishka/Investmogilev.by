// // -----------------------------------------------------------------------
// // <copyright file="IIspolcomFixesUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IIspolcomFixesUoW
	{
		void OnWaitIspolcomFixesExit();

		void OnWaitIspolcomFixesEntry();

		bool CouldIspolcomFixUpdate();

		bool CouldIspolcomFixUpdateAndLeave();
	}
}