using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
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