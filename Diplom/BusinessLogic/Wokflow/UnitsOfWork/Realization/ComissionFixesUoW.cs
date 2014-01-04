using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    public class ComissionFixesUoW : BaseProjectUoW, IComissionFixesUoW
    {
        public ComissionFixesUoW(Project currentProject,
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

        public void OnWaitComissionFixesExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnWaitComissionFixesEntry()
        {
            throw new System.NotImplementedException();
        }

        public bool CouldUpdateComissionFix()
        {
            return true;
        }

        public bool CouldUpdateComissionFixAndLeave()
        {
            return true;
        }
    }
}