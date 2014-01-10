using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow
{
    public class ProjectStateContext : IStateContext
    {
        public Project CurrentProject { get; set; }
        public IRepository Repository { get; set; }
        public IUserNotification UserNotification { get; set; }
        public IAdminNotification AdminNotification { get; set; }
        public IInvestorNotification InvestorNotification { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}