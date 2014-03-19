// // -----------------------------------------------------------------------
// // <copyright file="OnComissionUoW.cs" author="Andrei Tserakhau">
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

	[State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.OnComission)]
	internal class OnComissionUoW : BaseProjectUoW, IOnComissionUoW, IState
	{
		public OnComissionUoW(Project currentProject,
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

		public OnComissionUoW(ProjectStateContext context)
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

		public void OnOnComissionEntry()
		{
			Comission comission =
				Repository.All<Comission>(c => c.CommissionTime > DateTime.Now && c.Type == ComissionType.Comission)
					.First();
			if (comission.ProjectIds == null)
			{
				comission.ProjectIds = new List<string>();
			}
			if (!comission.ProjectIds.Contains(CurrentProject._id))
			{
				comission.ProjectIds.Add(CurrentProject._id);
				Repository.Update(comission);
			}

			InvestorNotification.Comission(comission, CurrentProject);
			AdminNotification.Comission(comission, CurrentProject);
			ProcessMoving(ProjectWorkflow.State.OnComission, "Проект ожидает комиссию");
		}

		public void OnOnComissionExit()
		{
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.ComissionFix, ProjectStatesConstants.OnComission,
			ProjectStatesConstants.WaitComissionFixes)]
		public bool CouldComissionFix()
		{
			return CurrentProject.Tasks.Any(t => t.Step == ProjectWorkflow.State.WaitComissionFixes);
		}

		[Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
			ProjectTriggersConstants.ToIspolcom, ProjectStatesConstants.OnComission,
			ProjectStatesConstants.WaitIspolcom)]
		public bool CouldToIspolcom()
		{
			return CurrentProject.Tasks.All(t => t.Step != ProjectWorkflow.State.WaitComissionFixes);
		}

		public IStateContext Context { get; set; }

		public void OnEntry()
		{
			OnOnComissionEntry();
		}

		public void OnExit()
		{
			OnOnComissionExit();
		}
	}
}