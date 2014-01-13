using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.WaitIspolcomFixes)]
    internal class IspolcomFixesUoW : BaseProjectUoW, IIspolcomFixesUoW, IState
    {
        public IspolcomFixesUoW(Project currentProject,
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

        public IspolcomFixesUoW(ProjectStateContext context)
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

        public void OnWaitIspolcomFixesExit()
        {
        }

        public void OnWaitIspolcomFixesEntry()
        {
            if (CurrentProject.WorkflowState.CurrentState == ProjectWorkflow.State.WaitIspolcomFixes)
            {
                AdminNotification.UpdateIspolcomFix(CurrentProject);
                InvestorNotification.UpdateIspolcomFix(CurrentProject);
            }
            else
            {
                InvestorNotification.IspolcomFixNeeded(CurrentProject);
            }

            ProcessMoving(ProjectWorkflow.State.WaitIspolcomFixes, "Обновление состояния оиждает исправлений");
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.IspolcomFixUpdate, ProjectStatesConstants.WaitComissionFixes,
            ProjectStatesConstants.WaitComissionFixes)]
        public bool CouldIspolcomFixUpdate()
        {
            return CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitIspolcomFixes && !t.IsComplete);
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.IspolcomFixUpdate, ProjectStatesConstants.WaitComissionFixes,
            ProjectStatesConstants.WaitIspolcom)]
        public bool CouldIspolcomFixUpdateAndLeave()
        {
            return !CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitIspolcomFixes && !t.IsComplete);
        }

        public IStateContext Context { get; set; }

        public void OnEntry()
        {
            OnWaitIspolcomFixesEntry();
        }

        public void OnExit()
        {
            OnWaitIspolcomFixesExit();
        }
    }
}