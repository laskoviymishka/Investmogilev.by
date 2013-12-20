using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Notification;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork
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
            AdminNotification.NotificateFill();
        }

        public void OnOpenEntry()
        {
            UserNotification.NotificateOpen();
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