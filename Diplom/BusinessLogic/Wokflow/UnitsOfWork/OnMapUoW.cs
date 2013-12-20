using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Notification;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public class OnMapUoW : BaseProjectUoW, IOnMapUoW
    {
        public OnMapUoW(Project currentProject,
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

        public void OnMapExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnMapEntry()
        {
            throw new System.NotImplementedException();
        }

        public bool FromOnComissionToOnMap()
        {
            return IsAdmin;
        }

        public bool FromOnIspolcomToOnMap()
        {
            return IsAdmin;
        }

        public bool FromOnMapToOnMap()
        {
            return IsAdmin || IsUser;
        }

        public bool FromWaitComissionFixesToOnMap()
        {
            return IsAdmin;
        }

        public bool FromWaitIspolcomFixesToOnMap()
        {
            return IsAdmin;
        }

        public bool FromOpenToOnMap()
        {
            return IsUser;
        }

        public bool FromOnMapToOpen()
        {
            return IsAdmin;
        }

        public bool FromOnMapToInvestorApprove()
        {
            return CurrentProject.Responses != null && CurrentProject.Responses.Any();
        }
    }
}