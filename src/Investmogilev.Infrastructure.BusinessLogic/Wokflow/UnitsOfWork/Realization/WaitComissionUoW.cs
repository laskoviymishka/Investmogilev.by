// // -----------------------------------------------------------------------
// // <copyright file="WaitComissionUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Investmogilev.Infrastructure.BusinessLogic.Notification;
	using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.Repository;
	using Investmogilev.Infrastructure.Common.State;
	using Investmogilev.Infrastructure.Common.State.StateAttributes;

	#endregion

	[State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.WaitComission)]
	internal class WaitComissionUoW : BaseProjectUoW, IWaitComissionUoW, IState
	{
		public WaitComissionUoW(Project currentProject,
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

		public WaitComissionUoW(ProjectStateContext context)
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

		public IStateContext Context { get; set; }

		public void OnEntry()
		{
			OnWaitComissionEntry();
		}

		public void OnExit()
		{
			OnWaitComissionExit();
		}

		public void OnWaitComissionExit()
		{
		}

		public void OnWaitComissionEntry()
		{
			InvestorNotification.WaitComission(CurrentProject);
			AdminNotification.WaitComission(CurrentProject);
			ProcessMoving(ProjectWorkflow.State.WaitComission, "Проект ожидает комиссию");
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.Comission, ProjectStatesConstants.WaitComission, ProjectStatesConstants.OnComission
			)]
		public bool CouldComission()
		{
			return
				Repository.All<Comission>().Any(c => c.CommissionTime > DateTime.Now && c.Type == ComissionType.Comission)
				&& Roles.Contains(ADMIN_ROLE);
		}
	}
}