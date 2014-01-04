using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class OnIspolcomUoW : BaseProjectUoW, IOnIspolcomUoW
    {
        public OnIspolcomUoW(Project currentProject,
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

        public void OnOnIspolcomExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnOnIspolcomEntry()
        {
            throw new System.NotImplementedException();
        }

        public bool CouldToMinEconomy()
        {
            return true;
        }

        public bool CouldToIspolcomFix()
        {
            return true;
        }
    }
}