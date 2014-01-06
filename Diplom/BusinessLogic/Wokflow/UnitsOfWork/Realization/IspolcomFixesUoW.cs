using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class IspolcomFixesUoW : BaseProjectUoW, IIspolcomFixesUoW
    {
        public IspolcomFixesUoW(Project currentProject,
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

        public void OnWaitIspolcomFixesExit()
        {
        }

        public void OnWaitIspolcomFixesEntry()
        {
            if (CurrentProject.WorkflowState.CurrentState == ProjectWorkflow.State.WaitIspolcomFixes)
            {
                AdminNotification.UpdateIspolcomFix(CurrentProject);
                InvestorNotification.UpdateIspolcomFix(CurrentProject);
            }
            else
            {
                InvestorNotification.IspolcomFixNeeded(CurrentProject);
            }

            ProcessMoving(ProjectWorkflow.State.WaitIspolcomFixes, "Обновление состояния оиждает исправлений");
        }

        public bool CouldIspolcomFixUpdate()
        {
            return CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitIspolcomFixes && !t.IsComplete);
        }

        public bool CouldIspolcomFixUpdateAndLeave()
        {
            return !CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitIspolcomFixes && !t.IsComplete);
        }
    }
}