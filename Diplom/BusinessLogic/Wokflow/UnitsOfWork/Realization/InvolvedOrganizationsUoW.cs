using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class InvolvedOrganizationsUoW : BaseProjectUoW, IInvolvedorganizationsUoW
    {
        public InvolvedOrganizationsUoW(Project currentProject,
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

        public void OnInvolvedOrganizationsExit()
        {
        }

        public void OnInvolvedOrganizationsEntry()
        {
            InvestorNotification.InvolvedOrganizationUpdate(CurrentProject);
            AdminNotification.InvolvedOrganizationUpdate(CurrentProject);
            ProcessMoving(ProjectWorkflow.State.InvolvedOrganizations, "Обход заинтерисованных организаций");
        }

        public bool CouldInvolvedOrganizationUpdate()
        {
            return CurrentProject.Tasks.Any(t => t.Type == TaskTypes.InvolvedOrganiztion && t.TaskReport == null);
        }

        public bool CouldInvolvedOrganizationUpdateAndLeave()
        {
            return !CurrentProject.Tasks.Any(t => t.Type == TaskTypes.InvolvedOrganiztion && t.TaskReport == null);
        }
    }
}