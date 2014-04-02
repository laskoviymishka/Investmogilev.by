// // -----------------------------------------------------------------------
// // <copyright file="IInvolvedorganizationsUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IInvolvedorganizationsUoW
	{
		void OnInvolvedOrganizationsExit();
		void OnInvolvedOrganizationsEntry();
		bool CouldInvolvedOrganizationUpdate();
		bool CouldInvolvedOrganizationUpdateAndLeave();
	}
}