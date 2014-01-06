using System;
using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class WaitComissionUoW :BaseProjectUoW, IWaitComissionUoW
    {
        public WaitComissionUoW(Project currentProject,
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

        public void OnWaitComissionExit()
        {
        }

        public void OnWaitComissionEntry()
        {
            InvestorNotification.WaitComission(CurrentProject);
            AdminNotification.WaitComission(CurrentProject);
            ProcessMoving(ProjectWorkflow.State.WaitComission, "Проект ожидает комиссию");
        }

        public bool CouldComission()
        {
            return Repository.All<Comission>(c => c.CommissionTime > DateTime.Now && c.Type == ComissionType.Comission) != null;
        }
    }
}