using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class OnIspolcomUoW : BaseProjectUoW, IOnIspolcomUoW
    {
        public OnIspolcomUoW(Project currentProject,
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

        public void OnOnIspolcomExit()
        {
            throw new NotImplementedException();
        }

        public void OnOnIspolcomEntry()
        {
            var comission = Repository.GetOne<Comission>(c => c.CommissionTime > DateTime.Now && c.Type == ComissionType.Ispolcom);
            if (comission.ProjectIds == null)
            {
                comission.ProjectIds = new List<string>();
            }

            if (!comission.ProjectIds.Contains(CurrentProject._id))
            {
                comission.ProjectIds.Add(CurrentProject._id);
                Repository.Update(comission);
            }

            AdminNotification.OnIspolcom(comission, CurrentProject);
            InvestorNotification.OnIspolcom(comission, CurrentProject);
            ProcessMoving(ProjectWorkflow.State.OnIspolcom, "Переход в состояние на исполкоме");
        }

        public bool CouldToMinEconomy()
        {
            return Roles.Contains("Admin");
        }

        public bool CouldToIspolcomFix()
        {
            return true;
        }
    }
}