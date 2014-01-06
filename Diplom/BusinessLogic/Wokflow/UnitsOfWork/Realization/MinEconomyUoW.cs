using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

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
        }

        public void OnInMinEconomyEntry()
        {
            InvestorNotification.InMinEconomy(CurrentProject);
            ProcessMoving(ProjectWorkflow.State.InMinEconomy, "Проект направлен на рассмотрение в министерство эконмомики");
        }

        public bool CouldMinEconomyResponsed()
        {
            return Roles.Contains("Admin");
        }
    }
}