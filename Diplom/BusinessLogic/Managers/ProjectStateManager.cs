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

            if (StateManager(response.ResponsedProjectId).CanFire(ProjectTriggers.ResponseFromInvest))
            {
                RepositoryContext.Current.Update(project);
                StateManager(response.ResponsedProjectId).Fire(ProjectTriggers.ResponseFromInvest);
            }
            else
            {
                if (project.WorkflowState.CurrenState == ProjectStates.WaitForAdminInvestorApprove)
                {
                    RepositoryContext.Current.Update(project);
                }
            }

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
            var state = StateManager(projectId);
            if (state.CanFire(ProjectTriggers.CompletePlanRealization))
            {
                state.Fire(ProjectTriggers.CompletePlanRealization);
            }
            else
            {
                if (state.CanFire(ProjectTriggers.CompleteDefectPlan))
                {
                    state.Fire(ProjectTriggers.CompleteDefectPlan);
                }
                else
                {
                    throw new InvalidOperationException("Операция не может быть произведена");
                }
            }
        }

        public void CompleteFillPlan(string projectId)
        {
            var state = StateManager(projectId);
            if (state.CanFire(ProjectTriggers.CompleteFillPlan))
            {
                state.Fire(ProjectTriggers.CompleteFillPlan);
            }
            else
            {
                if (state.CanFire(ProjectTriggers.FillDefectPlan))
                {
                    state.Fire(ProjectTriggers.FillDefectPlan);
                }
                else
                {
                    throw new InvalidOperationException("Операция не может быть произведена");
                }
            }
        }

        public void ApprovePlanByAdmin(string projectId)
        {
            var state = StateManager(projectId);
            if (state.CanFire(ProjectTriggers.ApprovePlanByAdmin))
            {
                state.Fire(ProjectTriggers.ApprovePlanByAdmin);
            }
            else
            {
                if (state.CanFire(ProjectTriggers.ApproveDefectPlanByAdmin))
                {
                    state.Fire(ProjectTriggers.ApproveDefectPlanByAdmin);
                }
                else
                {
                    throw new InvalidOperationException("Операция не может быть произведена");
                }
            }
        }

        public void FillProject(string projectId, Project project)
        {
            if (StateManager(projectId).CanFire(ProjectTriggers.FillProject))
            {
                RepositoryContext.Current.Update(project);
                StateManager(projectId).Fire(ProjectTriggers.FillProject);
            }
            else
            {
                RepositoryContext.Current.Update(project);
            }
        }

        public void ApproveResponseByInvestor(string projectId)
        {
            StateManager(projectId).Fire(ProjectTriggers.ApproveResponseByInvestor);
        }

        public void ApprovePlanByInvestor(string projectId)
        {
            var state = StateManager(projectId);
            if (state.CanFire(ProjectTriggers.ApprovePlanByInvestor))
            {
                state.Fire(ProjectTriggers.ApprovePlanByInvestor);
            }
            else
            {
                if (state.CanFire(ProjectTriggers.ApproveDefectPlanByInvestor))
                {
                    state.Fire(ProjectTriggers.ApproveDefectPlanByInvestor);
                }
                else
                {
                    throw new InvalidOperationException("Операция не может быть произведена");
                }
            }
        }

        public void UserApproveCompletion(string projectId)
        {
            var state = StateManager(projectId);
            if (state.CanFire(ProjectTriggers.ApprovePlanCompleteByUser))
            {
                state.Fire(ProjectTriggers.ApprovePlanCompleteByUser);
            }
            else
            {
                if (state.CanFire(ProjectTriggers.ApproveCompleteDefectPlanByUser))
                {
                    state.Fire(ProjectTriggers.ApproveCompleteDefectPlanByUser);
                }
                else
                {
                    throw new InvalidOperationException("Операция не может быть произведена");
                }
            }
        }

        public void AdminApproveCompletion(string projectId)
        {
            var state = StateManager(projectId);
            if (state.CanFire(ProjectTriggers.ApprovePlanCompleteByAdmin))
            {
                state.Fire(ProjectTriggers.ApprovePlanCompleteByAdmin);
            }
            else
            {
                if (state.CanFire(ProjectTriggers.ApproveCompleteDefectPlanByAdmin))
                {
                    state.Fire(ProjectTriggers.ApproveCompleteDefectPlanByAdmin);
                }
                else
                {
                    throw new InvalidOperationException("Операция не может быть произведена");
                }
            }
            StateManager(projectId).Fire(ProjectTriggers.ApprovePlanCompleteByAdmin);
        }

        public void CreateProject(Project project, string editor)
        {
            if (project.WorkflowState == null)
            {
                project.WorkflowState = new WorkflowEntity()
                {
                    CurrenState = ProjectStates.WaitForAssignee,
                    ChangeHistory = new List<History>()
                    {
                        new History()
                        {
                            EditingTime = DateTime.Now,
                            Editor = editor,
                            FromState = ProjectStates.WaitForAssignee,
                            ToState = ProjectStates.WaitForAssignee
                        }
                    }
                };
            }

            RepositoryContext.Current.Add<Project>(project);
        }

        public void BackToTasks(string projectId)
        {
            var state = StateManager(projectId);
            if (state.CanFire(ProjectTriggers.RejectPlanCompleteByUser))
            {
                state.Fire(ProjectTriggers.RejectPlanCompleteByUser);
            }
            else
            {
                if (state.CanFire(ProjectTriggers.RejectPlanCompleteByAdmin))
                {
                    state.Fire(ProjectTriggers.RejectPlanCompleteByAdmin);
                }
            }

            if (state.CanFire(ProjectTriggers.RejectCompleteDefectPlanByUser))
            {
                state.Fire(ProjectTriggers.RejectCompleteDefectPlanByUser);
            }
            else
            {
                if (state.CanFire(ProjectTriggers.RejectCompleteDefectPlanByAdmin))
                {
                    state.Fire(ProjectTriggers.RejectCompleteDefectPlanByAdmin);
                }
            }
        }
    }
}