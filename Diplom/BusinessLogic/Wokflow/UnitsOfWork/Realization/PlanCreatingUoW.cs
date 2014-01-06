using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class PlanCreatingUoW : BaseProjectUoW, IPlanCreatingUoW
    {
        public PlanCreatingUoW(Project currentProject,
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

        public void OnPlanCreatingExit()
        {
        }

        public void OnPlanCreatingEntry()
        {
            if (CurrentProject.WorkflowState.CurrentState == ProjectWorkflow.State.PlanCreating)
            {
                AdminNotification.PlanCreatingUpdate(CurrentProject);
            }
            else
            {
                InvestorNotification.MinEconomyResponsed(CurrentProject);
            }

            ProcessMoving(ProjectWorkflow.State.PlanCreating, "проект перешел в стадию создания дорожной карты");
        }

        public bool CouldUpdatePlan()
        {
            return Roles.Contains("Investor");
        }

        public bool CouldApprovePlan()
        {
            return Roles.Contains("Admin") && CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.Realization && !t.IsComplete);
        }
    }
}