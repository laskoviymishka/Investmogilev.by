using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
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

        public void OnWaitIspolcomExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnWaitIspolcomEntry()
        {
            throw new System.NotImplementedException();
        }

        public bool CouldToIspolcom()
        {
            return true;
        }
    }
}