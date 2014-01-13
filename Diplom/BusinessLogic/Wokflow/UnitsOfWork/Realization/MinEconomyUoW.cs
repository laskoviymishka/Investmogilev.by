using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.InMinEconomy)]
    internal class MinEconomyUoW : BaseProjectUoW, IMinEconomyUoW, IState
    {
        public MinEconomyUoW(Project currentProject,
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

        public MinEconomyUoW(ProjectStateContext context)
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

        public void OnInMinEconomyExit()
        {
        }

        public void OnInMinEconomyEntry()
        {
            InvestorNotification.InMinEconomy(CurrentProject);
            ProcessMoving(ProjectWorkflow.State.InMinEconomy,
                "Проект направлен на рассмотрение в министерство эконмомики");
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.MinEconomyResponsed, ProjectStatesConstants.InMinEconomy,
            ProjectStatesConstants.PlanCreating)]
        public bool CouldMinEconomyResponsed()
        {
            return Roles.Contains("Admin");
        }

        public IStateContext Context { get; set; }

        public void OnEntry()
        {
            OnInMinEconomyEntry();
        }

        public void OnExit()
        {
            OnInMinEconomyExit();
        }
    }
}