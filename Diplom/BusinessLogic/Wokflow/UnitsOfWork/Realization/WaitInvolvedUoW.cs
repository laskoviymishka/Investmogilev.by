using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class WaitInvolvedUoW : BaseProjectUoW, IWaitInvolvedUoW
    {
        public WaitInvolvedUoW(Project currentProject,
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

        public void OnWaitInvolvedExit()
        {
        }

        public void OnWaitInvolvedEntry()
        {
            AdminNotification.WaitInvolved(CurrentProject);
            ProcessMoving(ProjectWorkflow.State.WaitInvolved, "Заполнение заинтерисованных организаций");
        }

        public bool CouldFillInvolvedOrganization()
        {
            return CurrentProject.Tasks.Any(t => t.Type == TaskTypes.InvolvedOrganiztion);
        }
    }
}