using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    public class OpenUoW : BaseProjectUoW, IOpenUoW
    {
        public OpenUoW(Project currentProject,
            IRepository repository,
            IUserNotification userNotification,
            IAdminNotification adminNotification,
            IInvestorNotification investorNotification,
            string userName,
            IEnumerable<string> roles)
            : base(currentProject,
           repository,
           userNotification,
           adminNotification,
           investorNotification,
           userName,
           roles)
        {
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

        public bool FromMapToOpen()
        {
            return IsAdmin;
        }

        public bool FromOpenToMap()
        {
            return IsUser;
        }
    }
}