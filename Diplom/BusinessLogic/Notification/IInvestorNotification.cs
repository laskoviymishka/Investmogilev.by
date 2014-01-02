using Invest.Common.Model.Project;

namespace BusinessLogic.Notification
{
    public interface IInvestorNotification
    {
        void InvestorResponsed(Project CurrentProject);

        void DocumentUpdate(Project CurrentProject);

        void ProjectAproved(Project CurrentProject);
    }
}
