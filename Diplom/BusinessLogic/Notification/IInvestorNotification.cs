using Invest.Common.Model.Project;

namespace BusinessLogic.Notification
{
    public interface IInvestorNotification
    {
        void InvestorResponsed(Project project);

        void DocumentUpdate(Project project);

        void ProjectAproved(Project project);

        void InvolvedOrganizationUpdate(Project project);

        void Comission(Comission comission, Project project);

        void WaitComission(Project project);

        void UpdateComissionFix(Project project);

        void ComissionFixNeeded(Project project);

        void WaitIspolcom(Project project);
    }
}
