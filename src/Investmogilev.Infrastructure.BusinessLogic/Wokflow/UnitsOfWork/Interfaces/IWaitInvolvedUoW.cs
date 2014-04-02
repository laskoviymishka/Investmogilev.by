// // -----------------------------------------------------------------------
// // <copyright file="IWaitInvolvedUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IWaitInvolvedUoW
	{
		void OnWaitInvolvedExit();

		void OnWaitInvolvedEntry();

		bool CouldFillInvolvedOrganization();
	}
}