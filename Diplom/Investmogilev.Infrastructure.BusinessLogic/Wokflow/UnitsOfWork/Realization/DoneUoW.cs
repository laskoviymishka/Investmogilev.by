using System.Collections.Generic;
using Investmogilev.Infrastructure.BusinessLogic.Notification;
using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Repository;
using Investmogilev.Infrastructure.Common.State;
using Investmogilev.Infrastructure.Common.State.StateAttributes;

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.Done)]
    internal class DoneUoW : BaseProjectUoW, IDoneUoW, IState
    {
        public DoneUoW(Project currentProject,
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

        public DoneUoW(ProjectStateContext context)
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

        public void OnDoneExit()
        {
        }

        public void OnDoneEntry()
        {
            AdminNotification.Done(CurrentProject);
            InvestorNotification.Done(CurrentProject);

            ProcessMoving(ProjectWorkflow.State.Done, "Проект завершен");
        }

        public IStateContext Context { get; set; }

        public void OnEntry()
        {
            OnDoneEntry();
        }

        public void OnExit()
        {
            OnDoneExit();
        }
    }
}