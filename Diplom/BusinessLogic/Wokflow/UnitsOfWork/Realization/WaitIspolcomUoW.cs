using System;
using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

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
        }

        public void OnWaitIspolcomEntry()
        {
            InvestorNotification.WaitIspolcom(CurrentProject);
            AdminNotification.WaitIspolcom(CurrentProject);
            ProcessMoving(ProjectWorkflow.State.WaitIspolcom, "Проект ожидает исполнительный комитет");
        }

        public bool CouldToIspolcom()
        {
            return Repository.GetOne<Comission>(c => c.CommissionTime > DateTime.Now && c.Type == ComissionType.Ispolcom) != null;
        }
    }
}