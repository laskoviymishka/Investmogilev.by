// // -----------------------------------------------------------------------
// // <copyright file="IAdminNotification.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	#region Using

	using Investmogilev.Infrastructure.Common.Model.Project;

	#endregion

	public interface IAdminNotification
	{
		void NotificateFill(Project project);
		void MapEntryNotificate();
		void NotificateReOpen(Project project);
		void InvestorApprovedNotificate(Project project);
		void InvestorResponsed(Project project);
		void DocumentUpdate(Project project);
		void WaitInvolved(Project project);
		void InvolvedOrganizationUpdate(Project project);
		void Comission(Comission comission, Project project);
		void WaitComission(Project project);
		void UpdateComissionFix(Project project);
		void WaitIspolcom(Project project);
		void OnIspolcom(Comission comission, Project project);
		void UpdateIspolcomFix(Project project);
		void Realization(Project project);
		void PlanCreatingUpdate(Project project);
		void Done(Project project);
	}
}