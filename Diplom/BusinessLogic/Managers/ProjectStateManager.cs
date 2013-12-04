using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Invest.Common.Model.ProjectModels;
using Invest.Common.Notification;
using Invest.Common.State;
using MongoRepository;
using BusinessLogic.Notification;

namespace BusinessLogic.Managers
{
    public class ProjectStateManager
    {
        #region Private field

        private readonly INotification _mailNotification = new MailNotification();
        private readonly PortalNotificationHub _portalNotification = new PortalNotificationHub();
        private ProjectStateMachine _projectStateMachine;
        private string _userName;
        private string[] _userRoles;

        #endregion

        #region Constructor

        public ProjectStateManager()
        {
        }

        public ProjectStateManager(string userName, string[] userRoles)
        {
            _userName = userName;
            _userRoles = userRoles;
        }

        #endregion


        private ProjectStateMachine StateManager(string projectId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            _projectStateMachine = new ProjectStateMachine(project, _userName, _userRoles);
            return _projectStateMachine;
        }

        public void SetContext(string userName, string[] userRoles)
        {
            _userName = userName;
            _userRoles = userRoles;
        }

        #region Assignee

        public bool RemoveAssignee(string unassigneUsername, string unassigneeUserMail, string editorUsername)
        {
            var projects = RepositoryContext.Current.All<Project>(p => p.InvestorUser.ToLower() == unassigneUsername.ToLower());
            if (projects != null && projects.Any())
            {
                foreach (Project project in projects)
                {
                    project.InvestorUser = "";

                    var response = project.Responses.FirstOrDefault(r => r.InvestorEmail.ToLower() == unassigneeUserMail);

                    if (response != null)
                    {
                        project.Responses.Remove(response);
                    }

                    RepositoryContext.Current.Update(project);
                    StateManager(project._id).Fire(ProjectTriggers.RejectInvestorByAdmin);
                }

                return true;
            }
            else
            {
                projects = RepositoryContext.Current.All<Project>(p => p.AssignUser.ToLower() == unassigneUsername.ToLower());
                if (projects != null)
                {
                    foreach (Project project in projects)
                    {
                        project.AssignUser = "";

                        RepositoryContext.Current.Update(project);
                        StateManager(project._id).Fire(ProjectTriggers.RejectAssignUser);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool AssignProjectToUser(string projectId, string assignedUser)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            project.AssignUser = assignedUser;
            RepositoryContext.Current.Update(project);
            StateManager(projectId).Fire(ProjectTriggers.AssigneeUser);
            return true;
        }

        #endregion

        #region Responses

        public bool ResponseToProject(InvestorResponse response, string editorUserName)
        {
            var project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == response.ResponsedProjectId);

            if (project.Responses == null)
            {
                project.Responses = new List<InvestorResponse>();
            }

            project.Responses.Add(response);
            RepositoryContext.Current.Update(project);
            StateManager(response.ResponsedProjectId).Fire(ProjectTriggers.ResponseFromInvest);
            return true;
        }

        public bool ProcessVerifyResponse(string responseProjectId,
            string responseId,
            string editorUserName,
            string investorUserName,
            string investorPass,
            string investorMail)
        {
            var project = RepositoryContext.Current.GetOne<Project>(r => r._id == responseProjectId);
            if (project == null)
            {
                return false;
            }
            var response = project.Responses.FirstOrDefault(r => r.ResponseId == responseId);

            if (response != null) response.IsVerified = true;

            project.Responses = new List<InvestorResponse>() { response };
            project.InvestorUser = investorUserName;
            Membership.CreateUser(investorUserName, investorPass, investorMail);
            Roles.AddUserToRole(investorUserName, "Investor");
            RepositoryContext.Current.Update(project);
            StateManager(responseProjectId).Fire(ProjectTriggers.ApproveInvestorByAdmin);
            return true;
        }

        public bool ProcessVerifyResponse(string responseProjectId,
            string responseId,
            string editorUserName,
            string investorUserName)
        {
            var project = RepositoryContext.Current.GetOne<Project>(r => r._id == responseProjectId);
            if (project == null)
            {
                return false;
            }
            var response = project.Responses.FirstOrDefault(r => r.ResponseId == responseId);

            if (response != null) response.IsVerified = true;

            project.Responses = new List<InvestorResponse>() { response };
            project.InvestorUser = investorUserName;
            RepositoryContext.Current.Update(project);
            StateManager(responseProjectId).Fire(ProjectTriggers.ApproveInvestorByAdmin);
            return true;
        }

        public bool DeleteResponse(string responseId, string editorUser)
        {
            var investorResponse = InvestorResponses.FirstOrDefault(p => p.ResponseId == responseId);
            if (investorResponse != null)
            {
                var projectId = investorResponse.ResponsedProjectId;

                var project = RepositoryContext.Current.GetOne<Project>(r => r._id == projectId);
                if (project == null)
                {
                    return false;
                }
                project.Responses.Remove(investorResponse);
                StateManager(projectId).Fire(ProjectTriggers.RejectInvestorByAdmin);
                RepositoryContext.Current.Update(project);
            }

            return true;
        }

        private IEnumerable<InvestorResponse> InvestorResponses
        {
            get
            {
                var responsedProject =
                    RepositoryContext.Current.All<Project>(
                        r => r.Responses != null && r.WorkflowState.CurrenState == Invest.Common.State.ProjectStates.WaitForAdminInvestorApprove);
                var model = new List<InvestorResponse>();
                foreach (var project in responsedProject)
                {
                    model.AddRange(project.Responses);
                }
                return model;
            }
        }

        #endregion

        public void CompletePlan(string userName, string projectId)
        {
            StateManager(projectId).Fire(ProjectTriggers.CompletePlanRealization);
        }

        public void CompleteFillPlan(string projectId)
        {
            StateManager(projectId).Fire(ProjectTriggers.CompleteFillPlan);
        }

        public void ApprovePlanByAdmin(string projectId)
        {
            StateManager(projectId).Fire(ProjectTriggers.ApprovePlanByAdmin);
        }

        public void FillProject(string projectId)
        {
            StateManager(projectId).Fire(ProjectTriggers.FillProject);
        }

        public void ApproveResponseByInvestor(string projectId)
        {
            StateManager(projectId).Fire(ProjectTriggers.ApproveResponseByInvestor);
        }

        public void ApprovePlanByInvestor(string projectId)
        {
            StateManager(projectId).Fire(ProjectTriggers.ApprovePlanByInvestor);
        }

        public void UserApproveCompletion(string projectId)
        {
            StateManager(projectId).Fire(ProjectTriggers.ApprovePlanCompleteByUser);
        }

        public void AdminApproveCompletion(string projectId)
        {
            StateManager(projectId).Fire(ProjectTriggers.ApprovePlanCompleteByAdmin);
        }

    }
}