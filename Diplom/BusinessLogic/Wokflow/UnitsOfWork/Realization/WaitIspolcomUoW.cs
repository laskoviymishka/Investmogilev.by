using System;
using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.WaitIspolcom)]
    internal class WaitIspolcomUoW : BaseProjectUoW, IWaitIspolcomUoW, IState
    {
        public WaitIspolcomUoW(Project currentProject,
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

        public WaitIspolcomUoW(ProjectStateContext context)
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

        public IStateContext Context { get; set; }

        public void OnEntry()
        {
            OnWaitIspolcomEntry();
        }

        public void OnExit()
        {
            OnWaitIspolcomExit();
        }

        public void OnWaitIspolcomExit()
        {
        }

        public void OnWaitIspolcomEntry()
        {
            InvestorNotification.WaitIspolcom(CurrentProject);
            AdminNotification.WaitIspolcom(CurrentProject);
            ProcessMoving(ProjectWorkflow.State.WaitIspolcom, "Проект ожидает исполнительный комитет");
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.ToIspolcom, ProjectStatesConstants.WaitIspolcom,
            ProjectStatesConstants.OnIspolcom)]
        public bool CouldToIspolcom()
        {
            return
                Repository.All<Comission>(c => c.CommissionTime > DateTime.Now && c.Type == ComissionType.Ispolcom) !=
                null;
        }
    }
}