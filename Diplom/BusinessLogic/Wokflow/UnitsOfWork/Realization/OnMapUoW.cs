using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using System;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof(OnMapUoW) ,"test", "OnMap")]
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

        public OnMapUoW(ProjectStateContext context)
            :base(context.CurrentProject,
            context.Repository,
            context.UserNotification,
            context.AdminNotification,
            context.InvestorNotification,
            context.UserName,
            context.Roles)
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

        [Trigger("test", "Reject", "OnComission", "OnMap")]
        public bool FromOnComissionToOnMap()
        {
            return IsAdmin;
        }

        [Trigger("test", "Reject", "OnIspolcom", "OnMap")]
        public bool FromOnIspolcomToOnMap()
        {
            return IsAdmin;
        }

        [Trigger("test", "FillProject", "OnMap", "OnMap")]
        public bool FromOnMapToOnMap()
        {
            return IsAdmin || IsUser;
        }

        [Trigger("test", "Reject", "WaitComissionFixes", "OnMap")]
        public bool FromWaitComissionFixesToOnMap()
        {
            return IsAdmin;
        }

        [Trigger("test", "Reject", "WaitIspolcomFixes", "OnMap")]
        public bool FromWaitIspolcomFixesToOnMap()
        {
            return IsAdmin;
        }

        [Trigger("test", "FillProject", "Open", "OnMap")]
        public bool FromOpenToOnMap()
        {
            return IsUser;
        }

        [Trigger("test", "ReOpen", "OnMap", "Open")]
        public bool FromOnMapToOpen()
        {
            return IsAdmin;
        }

        [Trigger("test", "InvestorResponsed", "FromOnMap", "InvestorApprove")]
        public bool FromOnMapToInvestorApprove()
        {
            return true;
        }

        public void OnEntry()
        {
            OnMapEntry();
        }

        public void OnExit()
        {
            OnMapExit();
        }
    }
}