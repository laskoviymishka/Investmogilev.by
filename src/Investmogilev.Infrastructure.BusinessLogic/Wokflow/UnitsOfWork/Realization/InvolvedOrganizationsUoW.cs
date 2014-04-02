// // -----------------------------------------------------------------------
// // <copyright file="InvolvedOrganizationsUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization
{
	#region Using

	using System.Collections.Generic;
	using System.Linq;
	using Investmogilev.Infrastructure.BusinessLogic.Notification;
	using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.Repository;
	using Investmogilev.Infrastructure.Common.State;
	using Investmogilev.Infrastructure.Common.State.StateAttributes;

	#endregion

	[State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.InvestorApprove)]
	internal class InvolvedOrganizationsUoW : BaseProjectUoW, IInvolvedorganizationsUoW, IState
	{
		public InvolvedOrganizationsUoW(Project currentProject,
			IRepository repository,
			IUserNotification userNotification,
			IAdminNotification adminNotification,
			IInvestorNotification investorNotification,
			string userName,
			IEnumerable<string> roles)
			: this(new ProjectStateContext
			{
				UserNotification = userNotification,
				AdminNotification = adminNotification,
				InvestorNotification = investorNotification,
				CurrentProject = currentProject,
				Repository = repository,
				Roles = roles,
				UserName = userName
			})
		{
			if (CurrentProject != null)
			{
				if (currentProject.Responses == null)
				{
					currentProject.Responses = new List<InvestorResponse>();
				}
			}
		}

		public InvolvedOrganizationsUoW(ProjectStateContext context)
			: base(context.CurrentProject,
				context.Repository,
				context.UserNotification,
				context.AdminNotification,
				context.InvestorNotification,
				context.UserName,
				context.Roles)
		{
			Context = context;
		}

		public void OnInvolvedOrganizationsExit()
		{
		}

		public void OnInvolvedOrganizationsEntry()
		{
			InvestorNotification.InvolvedOrganizationUpdate(CurrentProject);
			AdminNotification.InvolvedOrganizationUpdate(CurrentProject);
			ProcessMoving(ProjectWorkflow.State.InvolvedOrganizations, "Обход заинтерисованных организаций");
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.InvolvedOrganizationUpdate, ProjectStatesConstants.InvolvedOrganizations,
			ProjectStatesConstants.InvolvedOrganizations)]
		public bool CouldInvolvedOrganizationUpdate()
		{
			return CurrentProject.Tasks.Any(t => t.Type == TaskTypes.InvolvedOrganiztion && t.TaskReport == null) && Roles.Contains(ADMIN_ROLE);
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.ToComission, ProjectStatesConstants.InvolvedOrganizations,
			ProjectStatesConstants.WaitComission)]
		public bool CouldInvolvedOrganizationUpdateAndLeave()
		{
			return !CurrentProject.Tasks.Any(t => t.Type == TaskTypes.InvolvedOrganiztion && t.TaskReport == null) && Roles.Contains(ADMIN_ROLE);
		}

		public IStateContext Context { get; set; }

		public void OnEntry()
		{
			OnInvolvedOrganizationsEntry();
		}

		public void OnExit()
		{
			OnInvolvedOrganizationsExit();
		}
	}
}