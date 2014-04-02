// // -----------------------------------------------------------------------
// // <copyright file="WaitIspolcomUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization
{
	#region Using

	using System;
	using System.Collections.Generic;
	using Investmogilev.Infrastructure.BusinessLogic.Notification;
	using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.Repository;
	using Investmogilev.Infrastructure.Common.State;
	using Investmogilev.Infrastructure.Common.State.StateAttributes;

	#endregion

	[State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.WaitIspolcom)]
	internal class WaitIspolcomUoW : BaseProjectUoW, IWaitIspolcomUoW, IState
	{
		public WaitIspolcomUoW(Project currentProject,
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

		public WaitIspolcomUoW(ProjectStateContext context)
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
			OnWaitIspolcomEntry();
		}

		public void OnExit()
		{
			OnWaitIspolcomExit();
		}

		public void OnWaitIspolcomExit()
		{
		}

		public void OnWaitIspolcomEntry()
		{
			InvestorNotification.WaitIspolcom(CurrentProject);
			AdminNotification.WaitIspolcom(CurrentProject);
			ProcessMoving(ProjectWorkflow.State.WaitIspolcom, "Проект ожидает исполнительный комитет");
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.ToIspolcom, ProjectStatesConstants.WaitIspolcom,
			ProjectStatesConstants.OnIspolcom)]
		public bool CouldToIspolcom()
		{
			return
				Repository.All<Comission>(c => c.CommissionTime > DateTime.Now && c.Type == ComissionType.Ispolcom) !=
				null;
		}
	}
}