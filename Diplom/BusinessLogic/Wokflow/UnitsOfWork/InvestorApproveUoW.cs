using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Notification;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public class InvestorApproveUoW : BaseProjectUoW, IInvestorApproveUoW
    {
        public InvestorApproveUoW(Project currentProject,
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

        public void OnInvestorApproveExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnInvestorApproveEntry()
        {
            throw new System.NotImplementedException();
        }

        public bool FromInvestorApproveToDocument()
        {
            throw new System.NotImplementedException();
        }

        public bool FromInvestorApproveToInvestorResponsed()
        {
            throw new System.NotImplementedException();
        }

        public bool FromOnMapToInvestorApprove()
        {
            throw new System.NotImplementedException();
        }
    }
}