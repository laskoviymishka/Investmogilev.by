using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.OnMap)]
    public class OpenUoW : BaseProjectUoW, IOpenUoW, IState
    {
        public OpenUoW(Project currentProject,
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

        public OpenUoW(ProjectStateContext context)
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

        public void OnOpenExit()
        {
            AdminNotification.NotificateFill(CurrentProject);
        }

        public void OnOpenEntry()
        {
            ProcessMoving(ProjectWorkflow.State.Open, "Проект перещел в состояние НА КАРТЕ");
            AdminNotification.NotificateReOpen();
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.ReOpen, ProjectStatesConstants.OnMap, ProjectStatesConstants.Open)]
        public bool FromMapToOpen()
        {
            return IsAdmin;
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.FillInformation, ProjectStatesConstants.Open, ProjectStatesConstants.OnMap)]
        public bool FromOpenToMap()
        {
            return IsUser || IsAdmin;
        }

        public IStateContext Context { get; set; }

        public void OnEntry()
        {
            OnOpenExit();
        }

        public void OnExit()
        {
            OnOpenExit();
        }
    }
}