using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using System;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    public class BaseProjectUoW
    {
        #region Protected const

        protected const string ADMIN_ROLE = "Admin";
        protected const string INVESTOR_ROLE = "Investor";
        protected const string RAYON_ROLE = "User";

        #endregion

        #region Protected Fields

        protected readonly Project CurrentProject;
        protected readonly IRepository Repository;
        protected readonly IUserNotification UserNotification;
        protected readonly IAdminNotification AdminNotification;
        protected readonly IInvestorNotification InvestorNotification;
        protected readonly string UserName;
        protected readonly IEnumerable<string> Roles;

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
                return CurrentProject.Tasks.Any(t => t.Step == CurrentProject.WorkflowState.CurrentState && !t.IsComplete);
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

            if (CurrentProject.WorkflowState.CurrentState == initialState)
            {
                throw new InvalidOperationException("UnExpected state " + initialState);
            }

            if (CurrentProject.WorkflowState.History == null)
            {
                CurrentProject.WorkflowState.History = new List<History>();
            }

            CurrentProject.WorkflowState.History.Add(new History()
            {
                EditingTime = DateTime.Now,
                Editor = UserName,
                To = initialState,
                From = CurrentProject.WorkflowState.CurrentState,
                Body = bodyMessage
            });

            CurrentProject.WorkflowState.CurrentState = initialState;
            Repository.Update<Project>(CurrentProject);
        }

        #endregion
    }
}