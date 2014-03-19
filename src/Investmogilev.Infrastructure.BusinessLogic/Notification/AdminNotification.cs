// // -----------------------------------------------------------------------
// // <copyright file="AdminNotification.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	#region Using

	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.State;

	#endregion

	public class AdminNotification : BaseNotificate, IAdminNotification
	{
		public AdminNotification()
			: base(RepositoryContext.Current)
		{
		}

		public void NotificateFill(Project currentProject)
		{
			SendMailFromDb(currentProject, currentProject, ProjectWorkflow.Trigger.FillInformation, UserType.Admin);
		}

		public void MapEntryNotificate()
		{
		}

		public void NotificateReOpen()
		{
		}

		public void InvestorApprovedNotificate(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.InvestorSelected, UserType.Admin);
		}

		public void InvestorResponsed(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.InvestorResponsed, UserType.Admin);
		}


		public void DocumentUpdate(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.DocumentUpdate, UserType.Admin);
		}

		public void WaitInvolved(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.InvolvedOrganizationUpdate, UserType.Admin);
		}


		public void InvolvedOrganizationUpdate(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.InvolvedOrganizationUpdate, UserType.Admin);
		}


		public void Comission(Comission comission, Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.Comission, UserType.Admin);
		}


		public void WaitComission(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ToComission, UserType.Admin);
		}


		public void UpdateComissionFix(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ComissionFixUpdate, UserType.Admin);
		}


		public void WaitIspolcom(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ToIspolcom, UserType.Admin);
		}


		public void OnIspolcom(Comission comission, Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.Ispolcom, UserType.Admin);
		}


		public void UpdateIspolcomFix(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.IspolcomFixUpdate, UserType.Admin);
		}


		public void PlanCreatingUpdate(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ApprovePlan, UserType.Admin);
		}


		public void Realization(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.UpdateRealization, UserType.Admin);
		}


		public void Done(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.UpdateRealization, UserType.Admin);
		}
	}
}