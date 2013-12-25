using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
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
            if (currentProject.Responses == null)
            {
                currentProject.Responses = new List<InvestorResponse>();
            }
        }

        public void OnInvestorApproveExit()
        {
        }

        public void OnInvestorApproveEntry()
        {
            CurrentProject = Repository.GetOne<Project>(p => p._id == CurrentProject._id);
            if (CurrentProject.WorkflowState.CurrentState == ProjectWorkflow.State.OnMap)
            {
                InvestorNotification.InvestorResponsed(CurrentProject);
                AdminNotification.InvestorResponsed(CurrentProject);
                UserNotification.InvestorResponsed(CurrentProject);
                ProcessMoving(ProjectWorkflow.State.InvestorApprove, "Переход с стадии одобрения инвестора");
            }
            else
            {
                if (CurrentProject.Responses.Count() > 1)
                {
                    InvestorNotification.InvestorResponsed(CurrentProject);
                    AdminNotification.InvestorResponsed(CurrentProject);
                    UserNotification.InvestorResponsed(CurrentProject);
                }
            }
        }

        public bool FromInvestorApproveToDocument()
        {
            return CurrentProject.Responses.Count(r => r.IsVerified) == 1;
        }

        public bool FromInvestorApproveToInvestorResponsed()
        {
            return CurrentProject.Responses.Any() && !CurrentProject.Responses.Any(r => r.IsVerified);
        }

        public bool FromOnMapToInvestorApprove()
        {
            return true;
        }
    }
}