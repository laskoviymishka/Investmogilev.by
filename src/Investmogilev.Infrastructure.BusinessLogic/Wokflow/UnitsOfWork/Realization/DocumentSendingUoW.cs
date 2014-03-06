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
	[State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.DocumentSending)]
	internal class DocumentSendingUoW : BaseProjectUoW, IDocumentSendingUoW, IState
	{
		public DocumentSendingUoW(Project currentProject,
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

		public DocumentSendingUoW(ProjectStateContext context)
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

		public void OnDocumentSendingEntry()
		{
			if (CurrentProject.WorkflowState.CurrentState == ProjectWorkflow.State.DocumentSending)
			{
				InvestorNotification.DocumentUpdate(CurrentProject);
				AdminNotification.DocumentUpdate(CurrentProject);
			}
			else
			{
				CurrentProject.InvestorUser = CurrentProject.Responses.Find(r => r.IsVerified).InvestorEmail;
				InvestorNotification.ProjectAproved(CurrentProject);
			}

			ProcessMoving(ProjectWorkflow.State.DocumentSending, "Проект в стадии сбора документов");
		}

		public void OnDocumentSendingExit()
		{
			InvestorNotification.DocumentUpdate(CurrentProject);
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.DocumentUpdate, ProjectStatesConstants.DocumentSending,
			ProjectStatesConstants.DocumentSending)]
		public bool CouldDocumentUpdate()
		{
			return CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.DocumentSending && !t.IsComplete);
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.DocumentUpdate, ProjectStatesConstants.DocumentSending,
			ProjectStatesConstants.WaitInvolved)]
		public bool CouldDocumentUpdateAndLeave()
		{
			return !CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.DocumentSending && !t.IsComplete);
		}

		public IStateContext Context { get; set; }

		public void OnEntry()
		{
			OnDocumentSendingEntry();
		}

		public void OnExit()
		{
			OnDocumentSendingExit();
		}
	}
}