using Invest.Common.Model.Project;

namespace BusinessLogic.Notification
{
    public interface IInvestorNotification
    {
        void InvestorResponsed(Project CurrentProject);
    }
}
