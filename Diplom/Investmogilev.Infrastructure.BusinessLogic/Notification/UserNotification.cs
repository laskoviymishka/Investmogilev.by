using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.State;

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
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