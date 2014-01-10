using Invest.Common;
using Invest.Common.Model.Common;
using Invest.Common.Model.Project;
using Invest.Common.State;

namespace BusinessLogic.Notification
{
    public class UserNotification : BaseNotificate, IUserNotification
    {
        public UserNotification()
            : base(RepositoryContext.Current)
        {
        }

        public void NotificateOpen(Project currentProject)
        {
            SendMailFromDb(currentProject, currentProject, ProjectWorkflow.Trigger.ReOpen, UserType.User);
        }

        public void InvestorResponsed(Project currentProject)
        {
            SendMailFromDb(currentProject, currentProject, ProjectWorkflow.Trigger.InvestorResponsed, UserType.User);
        }
    }
}