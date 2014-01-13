using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.PlanCreating)]
    internal class PlanCreatingUoW : BaseProjectUoW, IPlanCreatingUoW, IState
    {
        public PlanCreatingUoW(Project currentProject,
            IRepository repository,
            IUserNotification userNotification,
            IAdminNotification adminNotification,
            IInvestorNotification investorNotification,
            string userName,
            IEnumerable<string> roles)
            : this(new ProjectStateContext
            {
                UserNotification = userNotification,
                AdminNotification = adminNotification,
                InvestorNotification = investorNotification,
                CurrentProject = currentProject,
                Repository = repository,
                Roles = roles,
                UserName = userName
            })
        {
            if (CurrentProject != null)
            {
                if (currentProject.Responses == null)
                {
                    currentProject.Responses = new List<InvestorResponse>();
                }
            }
        }

        public PlanCreatingUoW(ProjectStateContext context)
            : base(context.CurrentProject,
                context.Repository,
                context.UserNotification,
                context.AdminNotification,
                context.InvestorNotification,
                context.UserName,
                context.Roles)
        {
            Context = context;
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

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.UpdatePlan, ProjectStatesConstants.PlanCreating,
            ProjectStatesConstants.PlanCreating)]
        public bool CouldUpdatePlan()
        {
            return Roles.Contains("Investor");
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.ApprovePlan, ProjectStatesConstants.PlanCreating,
            ProjectStatesConstants.Realization)]
        public bool CouldApprovePlan()
        {
            return Roles.Contains("Admin") &&
                   CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.Realization && !t.IsComplete);
        }

        public IStateContext Context { get; set; }

        public void OnEntry()
        {
            OnPlanCreatingEntry();
        }

        public void OnExit()
        {
            OnPlanCreatingExit();
        }
    }
}