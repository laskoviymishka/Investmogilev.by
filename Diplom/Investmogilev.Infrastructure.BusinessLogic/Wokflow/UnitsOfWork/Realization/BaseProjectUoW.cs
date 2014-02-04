using System;
using System.Collections.Generic;
using System.Linq;
using Investmogilev.Infrastructure.BusinessLogic.Notification;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Repository;
using Investmogilev.Infrastructure.Common.State;

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    public class BaseProjectUoW
    {
        #region Protected const

        protected const string ADMIN_ROLE = "Admin";
        protected const string INVESTOR_ROLE = "Investor";
        protected const string RAYON_ROLE = "User";

        #endregion

        #region Protected Fields

        protected readonly IAdminNotification AdminNotification;
        protected readonly IInvestorNotification InvestorNotification;
        protected readonly IRepository Repository;
        protected readonly IEnumerable<string> Roles;
        protected readonly string UserName;
        protected readonly IUserNotification UserNotification;
        protected Project CurrentProject;

        #endregion

        #region Constructor

        protected BaseProjectUoW(Project currentProject,
            IRepository repository,
            IUserNotification userNotification,
            IAdminNotification adminNotification,
            IInvestorNotification investorNotification,
            string userName,
            IEnumerable<string> roles)
        {
            CurrentProject = currentProject;
            Repository = repository;
            UserNotification = userNotification;
            AdminNotification = adminNotification;
            InvestorNotification = investorNotification;
            UserName = userName;
            Roles = roles;
        }

        #endregion

        #region Protected Fields

        protected bool IsProjectInvestor
        {
            get { return CurrentProject.InvestorUser == UserName && Roles.Any(r => r == INVESTOR_ROLE); }
        }

        protected bool IsAdmin
        {
            get { return Roles.Any(r => r == ADMIN_ROLE); }
        }

        protected bool IsUser
        {
            get { return Roles.Any(r => r == RAYON_ROLE); }
        }

        protected bool IsAllTaskForStepComplete
        {
            get
            {
                return
                    CurrentProject.Tasks.Any(t => t.Step == CurrentProject.WorkflowState.CurrentState && !t.IsComplete);
            }
        }

        #endregion

        #region Guard

        protected void GuardCurrentProjectNotNull()
        {
            if (CurrentProject == null)
            {
                throw new ArgumentNullException("CurrentProject");
            }
        }

        #endregion

        #region Protected Helpers

        protected void ProcessMoving(ProjectWorkflow.State initialState, string bodyMessage)
        {
            GuardCurrentProjectNotNull();

            if (CurrentProject.WorkflowState.History == null)
            {
                CurrentProject.WorkflowState.History = new List<History>();
            }

            CurrentProject.WorkflowState.History.Add(new History
            {
                EditingTime = DateTime.Now,
                Editor = UserName,
                To = initialState,
                From = CurrentProject.WorkflowState.CurrentState,
                Body = bodyMessage
            });

            CurrentProject.WorkflowState.CurrentState = initialState;
            Repository.Update(CurrentProject);
        }

        #endregion
    }
}