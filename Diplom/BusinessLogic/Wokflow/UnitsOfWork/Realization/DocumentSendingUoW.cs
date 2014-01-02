using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using System.Linq;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class DocumentSendingUoW : BaseProjectUoW,IDocumentSendingUoW
    {
        public DocumentSendingUoW(Project currentProject,
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

        public void OnDocumentSendingEntry()
        {
            if (CurrentProject.WorkflowState.CurrentState == ProjectWorkflow.State.DocumentSending)
            {
                InvestorNotification.DocumentUpdate(CurrentProject);
                AdminNotification.DocumentUpdate(CurrentProject);
            }
            else
            {
                CurrentProject.InvestorUser = CurrentProject.Responses.Find(r => r.IsVerified).InvestorEmail;
                InvestorNotification.ProjectAproved(CurrentProject);
            }

            ProcessMoving(ProjectWorkflow.State.DocumentSending, "Проект в стадии сбора документов");
        }

        public void OnDocumentSendingExit()
        {
            throw new System.NotImplementedException();
        }

        public bool CouldDocumentUpdate()
        {
            return CurrentProject.Tasks.Any(t =>
                (
                t.Step == ProjectWorkflow.State.DocumentSending
                && (t.IsComplete && !t.TaskReport.Last<Report>().ReportResponse.IsApproved || !t.IsComplete)));
        }
    }
}