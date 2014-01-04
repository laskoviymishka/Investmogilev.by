using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class IspolcomFixesUoW : BaseProjectUoW, IIspolcomFixesUoW
    {
        public IspolcomFixesUoW(Project currentProject,
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

        public void OnWaitIspolcomFixesExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnWaitIspolcomFixesEntry()
        {
            throw new System.NotImplementedException();
        }

        public bool CouldIspolcomFixUpdate()
        {
            return true;
        }

        public bool CouldIspolcomFixUpdateAndLeave()
        {
            return true;
        }
    }
}