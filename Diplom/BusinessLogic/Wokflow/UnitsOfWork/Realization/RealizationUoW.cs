using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.Realization)]
    internal class RealizationUoW : BaseProjectUoW, IRealizationUoW, IState
    {
        public RealizationUoW(Project currentProject,
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

        public RealizationUoW(ProjectStateContext context)
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

        public void OnRealizationExit()
        {
        }

        public void OnRealizationEntry()
        {
            InvestorNotification.Realization(CurrentProject);
            AdminNotification.Realization(CurrentProject);

            ProcessMoving(ProjectWorkflow.State.Realization, "Проект теперь реализуется");
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.UpdateRealization, ProjectStatesConstants.Realization,
            ProjectStatesConstants.Realization)]
        public bool CouldUpdateRealization()
        {
            return CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.Realization && !t.IsComplete);
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.UpdateRealization, ProjectStatesConstants.Realization,
            ProjectStatesConstants.Done)]
        public bool CouldUpdateRealizationAndLeave()
        {
            return !CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.Realization && !t.IsComplete);
        }

        public IStateContext Context { get; set; }

        public void OnEntry()
        {
            OnRealizationEntry();
        }

        public void OnExit()
        {
            OnRealizationExit();
        }
    }
}