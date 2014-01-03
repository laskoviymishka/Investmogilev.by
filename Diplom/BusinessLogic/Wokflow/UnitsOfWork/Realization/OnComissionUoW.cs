using System;
using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class OnComissionUoW : BaseProjectUoW, IOnComissionUoW
    {
        public OnComissionUoW(Project currentProject,
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

        public void OnComissionEntry()
        {
            var comission = Repository.GetOne<Comission>(c => c.CommissionTime > DateTime.Now);
            if (comission.ProjectIds == null)
            {
                comission.ProjectIds = new List<string>();
            }

            comission.ProjectIds.Add(CurrentProject._id);
            Repository.Update(comission);
            InvestorNotification.Comission(comission, CurrentProject);
            AdminNotification.Comission(comission, CurrentProject);
            ProcessMoving(ProjectWorkflow.State.OnComission, "Проект ожидает комиссию");
        }
    }
}