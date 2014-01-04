using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class RealizationUoW :BaseProjectUoW, IRealizationUoW
    {
        public RealizationUoW(Project currentProject,
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

        public void OnRealizationExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnRealizationEntry()
        {
            throw new System.NotImplementedException();
        }

        public bool CouldUpdateRealization()
        {
            return true;
        }

        public bool CouldUpdateRealizationAndLeave()
        {
            return true;
        }
    }
}