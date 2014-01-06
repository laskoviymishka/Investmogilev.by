using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class DoneUoW : BaseProjectUoW, IDoneUoW
    {
        public DoneUoW(Project currentProject,
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

        public void OnDoneExit()
        {
        }

        public void OnDoneEntry()
        {
            AdminNotification.Done(CurrentProject);
            InvestorNotification.Done(CurrentProject);

            ProcessMoving(ProjectWorkflow.State.Done, "Проект завершен");
        }
    }
}