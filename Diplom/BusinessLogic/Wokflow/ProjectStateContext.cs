using System.Collections.Generic;
using Investmogilev.Infrastructure.BusinessLogic.Notification;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Repository;
using Investmogilev.Infrastructure.Common.State.StateAttributes;

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow
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