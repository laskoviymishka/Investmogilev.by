using Invest.Common.Model.Project;

namespace BusinessLogic.Notification
{
    public interface IAdminNotification
    {
        void NotificateFill(Project project);
        void MapEntryNotificate();
        void NotificateReOpen();

        void InvestorApprovedNotificate(Project project);

        void InvestorResponsed(Project project);
    }
}
