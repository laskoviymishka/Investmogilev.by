using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using System;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
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
        }

        public void OnMapEntry()
        {
            GuardCurrentProjectNotNull();
            if (CurrentProject.Address.Lat > 51 && CurrentProject.Address.Lat < 55 && CurrentProject.Address.Lng > 28 &&
                CurrentProject.Address.Lng < 32)
            {
                ProcessMoving(ProjectWorkflow.State.OnMap, "Проект перещел в состояние НА КАРТЕ");
                AdminNotification.MapEntryNotificate();
            }
            else
            {
                throw new InvalidOperationException("Адрес не верен, перепроверьте адрес");
            }
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