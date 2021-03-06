﻿// // -----------------------------------------------------------------------
// // <copyright file="UnitsOfWorkContainer.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow
{
	#region Using

	using System;
	using System.Collections.Generic;
	using Investmogilev.Infrastructure.BusinessLogic.Notification;
	using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces;
	using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.Repository;
	using Investmogilev.Infrastructure.Common.State.StateAttributes;

	#endregion

	public class UnitsOfWorkContainer : IUnitsOfWorkContainer
	{
		#region Protected Fields

		private readonly IAdminNotification _adminNotification;
		private readonly Project _currentProject;
		private readonly IInvestorNotification _investorNotification;
		private readonly IRepository _repository;
		private readonly IEnumerable<string> _roles;
		private readonly string _userName;
		private readonly IUserNotification _userNotification;

		#endregion

		public UnitsOfWorkContainer(Project currentProject,
			IRepository repository,
			IUserNotification userNotification,
			IAdminNotification adminNotification,
			IInvestorNotification investorNotification,
			string userName,
			IEnumerable<string> roles)
		{
			_currentProject = currentProject;
			_repository = repository;
			_userNotification = userNotification;
			_adminNotification = adminNotification;
			_investorNotification = investorNotification;
			_userName = userName;
			_roles = roles;

			OpenUoW = new OpenUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			OnMapUoW = new OnMapUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			InvestorApproveUoW = new InvestorApproveUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);

			WaitIspolcomUoW = new WaitIspolcomUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			WaitInvolvedUoW = new WaitInvolvedUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			WaitComissionUoW = new WaitComissionUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			RealizationUoW = new RealizationUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			PlanCreatingUoW = new PlanCreatingUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			OnIspolcomUoW = new OnIspolcomUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			OnComissionUoW = new OnComissionUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			MinEconomyUoW = new MinEconomyUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			IspolcomFixesUoW = new IspolcomFixesUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			InvolvedorganizationsUoW = new InvolvedOrganizationsUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			DocumentSendingUoW = new DocumentSendingUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			ComissionFixesUoW = new ComissionFixesUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);
			DoneUoW = new DoneUoW(_currentProject,
				_repository,
				_userNotification,
				_adminNotification,
				_investorNotification,
				_userName,
				_roles);

			var dict = new Dictionary<Type, IState>();
			dict.Add(OnMapUoW.GetType(), OnMapUoW);
			AttributeStateMachineBuilder.InitializeStates(dict);
		}

		public IComissionFixesUoW ComissionFixesUoW { get; private set; }
		public IDocumentSendingUoW DocumentSendingUoW { get; private set; }
		public IDoneUoW DoneUoW { get; private set; }
		public IInvestorApproveUoW InvestorApproveUoW { get; private set; }
		public IInvolvedorganizationsUoW InvolvedorganizationsUoW { get; private set; }
		public IIspolcomFixesUoW IspolcomFixesUoW { get; private set; }
		public IMinEconomyUoW MinEconomyUoW { get; private set; }
		public IOnComissionUoW OnComissionUoW { get; private set; }
		public IOnIspolcomUoW OnIspolcomUoW { get; private set; }
		public IOnMapUoW OnMapUoW { get; private set; }
		public IOpenUoW OpenUoW { get; private set; }
		public IPlanCreatingUoW PlanCreatingUoW { get; private set; }
		public IRealizationUoW RealizationUoW { get; private set; }
		public IWaitComissionUoW WaitComissionUoW { get; private set; }
		public IWaitInvolvedUoW WaitInvolvedUoW { get; private set; }
		public IWaitIspolcomUoW WaitIspolcomUoW { get; private set; }
	}
}