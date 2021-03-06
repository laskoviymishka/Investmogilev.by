﻿// // -----------------------------------------------------------------------
// // <copyright file="InvestorApproveUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization
{
	#region Using

	using System.Collections.Generic;
	using System.Linq;
	using Investmogilev.Infrastructure.BusinessLogic.Notification;
	using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.Repository;
	using Investmogilev.Infrastructure.Common.State;
	using Investmogilev.Infrastructure.Common.State.StateAttributes;

	#endregion

	[State(typeof(ProjectWorkflow.State), "test", ProjectStatesConstants.InvestorApprove)]
	public class InvestorApproveUoW : BaseProjectUoW, IInvestorApproveUoW, IState
	{
		public InvestorApproveUoW(Project currentProject,
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

		public InvestorApproveUoW(ProjectStateContext context)
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

		public void OnInvestorApproveExit()
		{
		}

		public void OnInvestorApproveEntry()
		{
			CurrentProject = Repository.GetOne<Project>(p => p._id == CurrentProject._id);
			if (CurrentProject.WorkflowState.CurrentState == ProjectWorkflow.State.OnMap)
			{
				ProcessMoving(ProjectWorkflow.State.InvestorApprove, "Переход с стадии одобрения инвестора");
			}

			AdminNotification.InvestorResponsed(CurrentProject);
			UserNotification.InvestorResponsed(CurrentProject);
			InvestorNotification.InvestorResponsed(CurrentProject);
		}

		[Trigger(typeof(ProjectWorkflow.Trigger), typeof(ProjectWorkflow.State), "test",
			ProjectTriggersConstants.InvestorSelected, ProjectStatesConstants.InvestorApprove,
			ProjectStatesConstants.DocumentSending)]
		public bool FromInvestorApproveToDocument()
		{
			return CurrentProject.Responses.Count(r => r.IsVerified) == 1 && CurrentProject.Tasks != null &&
				   CurrentProject.Tasks.Any();
		}

		[Trigger(typeof(ProjectWorkflow.Trigger), typeof(ProjectWorkflow.State), "test",
			ProjectTriggersConstants.InvestorResponsed, ProjectStatesConstants.InvestorApprove,
			ProjectStatesConstants.InvestorApprove)]
		public bool FromInvestorApproveToInvestorResponsed()
		{
			return Roles == null || !Roles.Any() && !CurrentProject.Responses.Any(r => r.IsVerified);
		}

		[Trigger(typeof(ProjectWorkflow.Trigger), typeof(ProjectWorkflow.State), "test",
			ProjectTriggersConstants.InvestorResponsed, ProjectStatesConstants.OnMap,
			ProjectStatesConstants.InvestorApprove)]
		public bool FromOnMapToInvestorApprove()
		{
			return Roles == null || !Roles.Any() && string.IsNullOrEmpty(UserName);
		}

		public IStateContext Context { get; set; }

		public void OnEntry()
		{
			OnInvestorApproveEntry();
		}

		public void OnExit()
		{
			OnInvestorApproveExit();
		}
	}
}