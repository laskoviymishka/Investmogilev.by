using System.Collections.Generic;
using System.Linq;
using Investmogilev.Infrastructure.BusinessLogic.Notification;
using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Repository;
using Investmogilev.Infrastructure.Common.State;
using Investmogilev.Infrastructure.Common.State.StateAttributes;

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization
{
	[State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.WaitComissionFixes)]
	public class ComissionFixesUoW : BaseProjectUoW, IComissionFixesUoW, IState
	{
		public ComissionFixesUoW(Project currentProject,
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

		public ComissionFixesUoW(ProjectStateContext context)
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

		public void OnWaitComissionFixesExit()
		{
		}

		public void OnWaitComissionFixesEntry()
		{
			if (CurrentProject.WorkflowState.CurrentState == ProjectWorkflow.State.WaitComissionFixes)
			{
				AdminNotification.UpdateComissionFix(CurrentProject);
				InvestorNotification.UpdateComissionFix(CurrentProject);
			}
			else
			{
				InvestorNotification.ComissionFixNeeded(CurrentProject);
			}

			ProcessMoving(ProjectWorkflow.State.WaitComissionFixes, "Обновление состояния");
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.ComissionFixUpdate, ProjectStatesConstants.WaitComissionFixes,
			ProjectStatesConstants.WaitComissionFixes)]
		public bool CouldUpdateComissionFix()
		{
			return CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitComissionFixes && !t.IsComplete);
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.ComissionFixUpdate, ProjectStatesConstants.WaitComissionFixes,
			ProjectStatesConstants.WaitIspolcom)]
		public bool CouldUpdateComissionFixAndLeave()
		{
			return !CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitComissionFixes && !t.IsComplete);
		}

		public IStateContext Context { get; set; }

		public void OnEntry()
		{
			OnWaitComissionFixesEntry();
		}

		public void OnExit()
		{
			OnWaitComissionFixesExit();
		}
	}
}