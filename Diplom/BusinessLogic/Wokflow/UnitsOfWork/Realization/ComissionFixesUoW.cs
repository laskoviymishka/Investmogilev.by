using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    public class ComissionFixesUoW : BaseProjectUoW, IComissionFixesUoW
    {
        public ComissionFixesUoW(Project currentProject,
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

        public void OnWaitComissionFixesExit()
        {

        }

        public void OnWaitComissionFixesEntry()
        {
            if (CurrentProject.WorkflowState.CurrentState == ProjectWorkflow.State.WaitComissionFixes)
            {
                AdminNotification.UpdateComissionFix(CurrentProject);
                InvestorNotification.UpdateComissionFix(CurrentProject);
            }
            else
            {
                InvestorNotification.ComissionFixNeeded(CurrentProject);
            }

            ProcessMoving(ProjectWorkflow.State.WaitComissionFixes, "Обновление состояния");
        }

        public bool CouldUpdateComissionFix()
        {
            return CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitComissionFixes && !t.IsComplete);
        }

        public bool CouldUpdateComissionFixAndLeave()
        {
            return !CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitComissionFixes && !t.IsComplete);
        }
    }
}