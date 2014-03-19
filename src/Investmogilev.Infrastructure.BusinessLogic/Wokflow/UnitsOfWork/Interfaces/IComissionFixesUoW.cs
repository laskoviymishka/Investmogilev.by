// // -----------------------------------------------------------------------
// // <copyright file="IComissionFixesUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IComissionFixesUoW
	{
		void OnWaitComissionFixesExit();

		void OnWaitComissionFixesEntry();

		bool CouldUpdateComissionFix();

		bool CouldUpdateComissionFixAndLeave();
	}
}