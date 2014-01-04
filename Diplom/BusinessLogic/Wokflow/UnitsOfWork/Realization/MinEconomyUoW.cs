using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class MinEconomyUoW : BaseProjectUoW, IMinEconomyUoW
    {
         public MinEconomyUoW(Project currentProject,
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

         public void OnInMinEconomyExit()
         {
             throw new System.NotImplementedException();
         }

         public void OnInMinEconomyEntry()
         {
             throw new System.NotImplementedException();
         }

         public bool CouldMinEconomyResponsed()
         {
             return true;
         }
    }
}