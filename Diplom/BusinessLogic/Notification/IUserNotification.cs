using Invest.Common.Model.Project;

namespace BusinessLogic.Notification
{
    public interface IUserNotification
    {
        void NotificateOpen(Project currentProject);
        void InvestorResponsed(Project currentProject);
    }
}