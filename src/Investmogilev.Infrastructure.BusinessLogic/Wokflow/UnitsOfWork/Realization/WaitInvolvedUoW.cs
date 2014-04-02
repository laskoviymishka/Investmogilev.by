// // -----------------------------------------------------------------------
// // <copyright file="WaitInvolvedUoW.cs" author="Andrei Tserakhau">
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

	[State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.DocumentSending)]
	internal class WaitInvolvedUoW : BaseProjectUoW, IWaitInvolvedUoW, IState
	{
		public WaitInvolvedUoW(Project currentProject,
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
		}

		public WaitInvolvedUoW(ProjectStateContext context)
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

		public IStateContext Context { get; set; }

		public void OnEntry()
		{
			OnWaitInvolvedEntry();
		}

		public void OnExit()
		{
			OnWaitInvolvedExit();
		}

		public void OnWaitInvolvedExit()
		{
		}

		public void OnWaitInvolvedEntry()
		{
			AdminNotification.WaitInvolved(CurrentProject);
			ProcessMoving(ProjectWorkflow.State.WaitInvolved, "Заполнение заинтерисованных организаций");
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.FillInvolvedOrganization, ProjectStatesConstants.WaitInvolved,
			ProjectStatesConstants.InvolvedOrganizations)]
		public bool CouldFillInvolvedOrganization()
		{
			return CurrentProject.Tasks.Any(t => t.Type == TaskTypes.InvolvedOrganiztion);
		}
	}
}