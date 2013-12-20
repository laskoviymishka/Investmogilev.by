using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Notification;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork
{
    class WaitIspolcomUoW : BaseProjectUoW, IWaitIspolcomUoW
    {
        public WaitIspolcomUoW(Project currentProject,
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
    }
}