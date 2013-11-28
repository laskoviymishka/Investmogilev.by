using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Invest.Common.Model.ProjectModels;
using Invest.Common.Notification;
using MongoRepository;

namespace BusinessLogic.Managers
{
    public class ProjectStateManager
    {
        #region Private field

        private readonly INotification _mailNotification = new MailNotification();

        #endregion

        public bool RemoveAssignee(string unassigneUsername, string unassigneeUserMail, string editorUsername)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p.InvestorUser.ToLower() == unassigneUsername.ToLower());
            if (project != null)
            {
                project.InvestorUser = "";

                var response = project.Responses.FirstOrDefault(r => r.InvestorEmail.ToLower() == unassigneeUserMail);

                if (response != null)
                {
                    project.Responses.Remove(response);
                }

                string fromState = project.WorkflowState.CurrenState;
                var workflow = project.WorkflowState;
                workflow.CurrenState = GreenFieldStates.Open;
                project.WorkflowState.ChangeHistory.Add(
                    new History()
                    {
                        EditingTime = DateTime.Now,
                        Editor = editorUsername,
                        FromState = fromState,
                        ToState = GreenFieldStates.Open
                    });

                RepositoryContext.Current.Update(project);
                return true;
            }
            else
            {
                project = RepositoryContext.Current.GetOne<Project>(p => p.AssignUser.ToLower() == unassigneUsername.ToLower());
                if (project != null)
                {
                    project.AssignUser = "";

                    RepositoryContext.Current.Update(project);
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
            return true;
        }

        public bool ResponseToProject(InvestorResponse response, string editorUserName)
        {
            var project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == response.ResponsedProjectId);

            project.WorkflowState.CurrenState = GreenFieldStates.WaitForVerifyResponse;
            if (project.WorkflowState.ChangeHistory == null)
            {
                project.WorkflowState.ChangeHistory = new List<History>();
            }
            project.WorkflowState.ChangeHistory.Add(
                new History()
                {
                    EditingTime = DateTime.Now,
                    Editor = editorUserName,
                    FromState = GreenFieldStates.Open,
                    ToState = GreenFieldStates.WaitForVerifyResponse
                });
            if (project.Responses == null)
            {
                project.Responses = new List<InvestorResponse>();
            }

            project.Responses.Add(response);
            RepositoryContext.Current.Update(project);
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

            string fromState = project.WorkflowState.CurrenState;
            var workflow = project.WorkflowState;
            workflow.CurrenState = GreenFieldStates.VerifyResponse;
            project.WorkflowState.ChangeHistory.Add(
                new History()
                {
                    EditingTime = DateTime.Now,
                    Editor = editorUserName,
                    FromState = fromState,
                    ToState = GreenFieldStates.VerifyResponse
                });
            project.InvestorUser = investorUserName;
            _mailNotification.NotificateUser("system",
                investorMail,
                "Вы откликнулись на проект",
                string.Format("Для вас создан аккаунт в системе. Ваш логин - {0}  ваш пароль - {1} .", investorUserName, investorPass));
            Membership.CreateUser(investorUserName, investorPass, investorMail);
            Roles.AddUserToRole(investorUserName, "Investor");
            RepositoryContext.Current.Update(project);
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
                string fromState = project.WorkflowState.CurrenState;
                var workflow = project.WorkflowState;
                project.Responses.Remove(investorResponse);
                workflow.CurrenState = GreenFieldStates.Open;
                project.WorkflowState.ChangeHistory.Add(
                    new History()
                    {
                        EditingTime = DateTime.Now,
                        Editor = editorUser,
                        FromState = fromState,
                        ToState = GreenFieldStates.Open
                    });

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
                        r => r.Responses != null && r.WorkflowState.CurrenState == GreenFieldStates.WaitForVerifyResponse);
                var model = new List<InvestorResponse>();
                foreach (var project in responsedProject)
                {
                    model.AddRange(project.Responses);
                }
                return model;
            }
        }
    }
}