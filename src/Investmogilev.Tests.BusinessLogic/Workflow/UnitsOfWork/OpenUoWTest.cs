// // -----------------------------------------------------------------------
// // <copyright file="OpenUoWTest.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Tests.BusinessLogic.Workflow.UnitsOfWork
{
	#region Using

	using System.Collections;
	using System.Collections.Generic;
	using Investmogilev.Infrastructure.BusinessLogic.Notification;
	using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.Repository;
	using Investmogilev.Infrastructure.Common.State;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using MongoDB.Bson;
	using Moq;

	#endregion

	/// <summary>
	///     This is a test class for OpenUoWTest and is intended
	///     to contain all OpenUoWTest Unit Tests
	/// </summary>
	[TestClass]
	public class OpenUoWTest
	{
		#region Private Fields

		private Mock<IAdminNotification> _adminNotification;
		private Project _currentProject;
		private Mock<IInvestorNotification> _investorNotification;
		private IList _projects;
		private IRepository _repository;
		private IEnumerable<string> _roles;
		private string _userName;
		private Mock<IUserNotification> _userNotification;

		#endregion

		#region Initialize

		[TestInitialize]
		public void MyTestInitialize()
		{
			_currentProject = new Project
			{
				_id = ObjectId.GenerateNewId().ToString(),
				Name = "testProjectName",
				Region = "testProjectRegion",
				InvestorUser = "",
				Address = new Address {Lat = 52, Lng = 53},
				WorkflowState = new Workflow
				{
					History = new List<History>(),
					CurrentState = ProjectWorkflow.State.Open
				}
			};
			_projects = new List<Project> {_currentProject};

			_repository = new MockMongoRepository(_projects);


			_userNotification = new Mock<IUserNotification>();
			_adminNotification = new Mock<IAdminNotification>();
			_investorNotification = new Mock<IInvestorNotification>();
			_userName = "test";
			_roles = new List<string>();
		}

		private OpenUoW CreateUoW()
		{
			return new OpenUoW(_currentProject,
				_repository,
				_userNotification.Object,
				_adminNotification.Object,
				_investorNotification.Object,
				_userName,
				_roles);
		}

		#endregion

		/// <summary>
		///     A test for OpenUoW Constructor
		/// </summary>
		[TestMethod]
		public void OpenUoWConstructorTest()
		{
			var target = CreateUoW();
			Assert.IsNotNull(_repository.GetOne<Project>(t => t._id == _currentProject._id));
			Assert.IsNotNull(target);
		}

		/// <summary>
		///     A test for FromMapToOpen
		/// </summary>
		[TestMethod]
		public void FromMapToOpenTest()
		{
			var target = CreateUoW();
			_currentProject.WorkflowState.CurrentState = ProjectWorkflow.State.OnMap;
			Assert.IsNotNull(_repository.GetOne<Project>(t => t._id == _currentProject._id));
			Assert.IsTrue(_repository.GetOne<Project>(t => t._id == _currentProject._id).WorkflowState.CurrentState ==
			              ProjectWorkflow.State.OnMap);
			Assert.IsFalse(target.FromMapToOpen());
			_roles = new[] {"Admin"};
			target = CreateUoW();
			Assert.IsTrue(target.FromMapToOpen());
		}

		/// <summary>
		///     A test for FromOpenToMap
		/// </summary>
		[TestMethod]
		public void FromOpenToMapTest()
		{
			var target = CreateUoW();
			_currentProject.WorkflowState.CurrentState = ProjectWorkflow.State.Open;
			Assert.IsNotNull(_repository.GetOne<Project>(t => t._id == _currentProject._id));
			Assert.IsTrue(_repository.GetOne<Project>(t => t._id == _currentProject._id).WorkflowState.CurrentState ==
			              ProjectWorkflow.State.Open);
			Assert.IsFalse(target.FromOpenToMap());
			_roles = new[] {"User"};
			target = CreateUoW();
			Assert.IsTrue(target.FromOpenToMap());
		}

		/// <summary>
		///     A test for OnOpenEntry
		/// </summary>
		[TestMethod]
		public void OnOpenEntryTest()
		{
			bool wasNotificated = false;
			_adminNotification.Setup(a => a.NotificateReOpen()).Callback(() => { wasNotificated = true; });

			_currentProject.WorkflowState.CurrentState = ProjectWorkflow.State.Open;
			var target = CreateUoW();
			target.OnOpenEntry();

			_currentProject.WorkflowState.CurrentState = ProjectWorkflow.State.Realization;
			target = CreateUoW();

			target.OnOpenEntry();

			Assert.IsTrue(
				_repository.GetOne<Project>(
					p => p._id == _currentProject._id).WorkflowState.CurrentState == ProjectWorkflow.State.Open);

			Assert.IsTrue(_repository.GetOne<Project>(
				p => p._id == _currentProject._id).WorkflowState.History.Count > 0);

			Assert.IsTrue(_repository.GetOne<Project>(
				p => p._id == _currentProject._id)
				.WorkflowState.History.Find(
					h =>
						h.Editor == _userName
						&& h.From == ProjectWorkflow.State.Realization
						&& h.To == ProjectWorkflow.State.Open) != null);

			Assert.IsTrue(wasNotificated);
		}

		/// <summary>
		///     A test for OnOpenExit
		/// </summary>
		[TestMethod]
		public void OnOpenExitTest()
		{
			bool wasNotificate = false;
			_adminNotification.Setup(a => a.NotificateFill(It.IsAny<Project>())).Callback(() => { wasNotificate = true; });

			var target = CreateUoW();

			target.OnOpenExit();
			Assert.IsTrue(wasNotificate);
		}
	}
}