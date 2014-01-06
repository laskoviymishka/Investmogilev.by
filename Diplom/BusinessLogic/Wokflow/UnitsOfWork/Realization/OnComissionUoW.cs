using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common;
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

        public void OnOnComissionEntry()
        {
            var comission = Repository.All<Comission>(c => c.CommissionTime > DateTime.Now && c.Type == ComissionType.Comission).First();
            if (comission.ProjectIds == null)
            {
                comission.ProjectIds = new List<string>();
            }
            if (!comission.ProjectIds.Contains(CurrentProject._id))
            {
                comission.ProjectIds.Add(CurrentProject._id);
                Repository.Update(comission);
            }

            InvestorNotification.Comission(comission, CurrentProject);
            AdminNotification.Comission(comission, CurrentProject);
            ProcessMoving(ProjectWorkflow.State.OnComission, "Проект ожидает комиссию");
        }

        public void OnOnComissionExit()
        {
        }

        public bool CouldComissionFix()
        {
            return CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitComissionFixes);
        }

        public bool CouldToIspolcom()
        {
            return CurrentProject.Tasks.All(t => t.Step != ProjectWorkflow.State.WaitComissionFixes);
        }
    }
}