using System;
using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof (ProjectWorkflow.State), "test", "OnMap")]
    public class OnMapUoW : BaseProjectUoW, IOnMapUoW
    {
        public OnMapUoW(Project currentProject,
            IRepository repository,
            IUserNotification userNotification,
            IAdminNotification adminNotification,
            IInvestorNotification investorNotification,
            string userName,
            IEnumerable<string> roles)
            : this(new ProjectStateContext
            {
                UserNotification = userNotification,
                AdminNotification = adminNotification,
                InvestorNotification = investorNotification,
                CurrentProject = currentProject,
                Repository = repository,
                Roles = roles,
                UserName = userName
            })
        {
            if (CurrentProject != null)
            {
                if (currentProject.Responses == null)
                {
                    currentProject.Responses = new List<InvestorResponse>();
                }
            }
        }

        public OnMapUoW(ProjectStateContext context)
            : base(context.CurrentProject,
                context.Repository,
                context.UserNotification,
                context.AdminNotification,
                context.InvestorNotification,
                context.UserName,
                context.Roles)
        {
            Context = context;
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

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.RejectDocument, ProjectStatesConstants.OnComission, ProjectStatesConstants.OnMap)]
        public bool FromOnComissionToOnMap()
        {
            return IsAdmin;
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.RejectDocument, ProjectStatesConstants.OnIspolcom, ProjectStatesConstants.OnMap)]
        public bool FromOnIspolcomToOnMap()
        {
            return IsAdmin;
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.UpdateInformation, ProjectStatesConstants.OnMap, ProjectStatesConstants.OnMap)]
        public bool FromOnMapToOnMap()
        {
            return IsAdmin || IsUser;
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.RejectDocument, ProjectStatesConstants.WaitComissionFixes,
            ProjectStatesConstants.OnMap)]
        public bool FromWaitComissionFixesToOnMap()
        {
            return IsAdmin;
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.RejectDocument, ProjectStatesConstants.WaitIspolcomFixes,
            ProjectStatesConstants.OnMap)]
        public bool FromWaitIspolcomFixesToOnMap()
        {
            return IsAdmin;
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.FillInformation, ProjectStatesConstants.Open, ProjectStatesConstants.OnMap)]
        public bool FromOpenToOnMap()
        {
            return IsUser || IsAdmin;
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.ReOpen, ProjectStatesConstants.OnMap, ProjectStatesConstants.Open)]
        public bool FromOnMapToOpen()
        {
            return IsAdmin;
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.InvestorResponsed, ProjectStatesConstants.OnMap,
            ProjectStatesConstants.InvestorApprove)]
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

        public IStateContext Context { get; set; }
    }
}