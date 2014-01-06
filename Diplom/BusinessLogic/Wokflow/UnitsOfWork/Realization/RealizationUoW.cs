using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

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
        }

        public void OnRealizationEntry()
        {
            InvestorNotification.Realization(CurrentProject);
            AdminNotification.Realization(CurrentProject);

            ProcessMoving(ProjectWorkflow.State.Realization, "Проект теперь реализуется");
        }

        public bool CouldUpdateRealization()
        {
            return CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.Realization && !t.IsComplete);
        }

        public bool CouldUpdateRealizationAndLeave()
        {
            return !CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.Realization && !t.IsComplete);
        }
    }
}