// // -----------------------------------------------------------------------
// // <copyright file="ProjectStateManagerTest.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Tests.BusinessLogic.Workflow
{
	#region Using

	using System.Collections;
	using System.Collections.Generic;
	using Investmogilev.Infrastructure.BusinessLogic.Notification;
	using Investmogilev.Infrastructure.BusinessLogic.Wokflow;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.State;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using MongoDB.Bson;
	using Moq;

	#endregion

	/// <summary>
	///     This is a test class for ProjectStateManagerTest and is intended
	///     to contain all ProjectStateManagerTest Unit Tests
	/// </summary>
	[TestClass]
	public class ProjectStateManagerTest
	{
		#region Private Fields

		private Mock<IAdminNotification> _adminNotification;
		private Project _currentProject;
		private Mock<IInvestorNotification> _investorNotification;
		private IList _projects;
		private MockMongoRepository _repository;
		private IEnumerable<string> _roles;
		private UnitsOfWorkContainer _unitOfWorksContainer;
		private string _userName;
		private Mock<IUserNotification> _userNotification;
		private ProjectWorkflowWrapper _workflow;

		#endregion

		[TestInitialize]
		public void MyTestInitialize()
		{
			_currentProject = new Project
			{
				_id = ObjectId.GenerateNewId().ToString(),
				Name = "testProjectName",
				Region = "testProjectRegion",
				InvestorUser = "",
				Address = new Address {Lat = 53, Lng = 30},
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
			_unitOfWorksContainer = new UnitsOfWorkContainer(_currentProject,
				_repository,
				_userNotification.Object,
				_adminNotification.Object,
				_investorNotification.Object,
				_userName,
				_roles);
		}


		/// <summary>
		///     A test for CreateProject
		/// </summary>
		[TestMethod]
		public void CreateProjectTest()
		{
		}

		/// <summary>
		///     A test for FillInformation
		/// </summary>
		[TestMethod]
		public void FillInformationTest()
		{
		}

		/// <summary>
		///     A test for ResponsedOnProject
		/// </summary>
		[TestMethod]
		public void ResponsedOnProjectTest()
		{
		}

		/// <summary>
		///     A test for StateManagerFactory
		/// </summary>
		[TestMethod]
		public void StateManagerFactoryTest()
		{
		}

		/// <summary>
		///     A test for ProjectStateManager Constructor
		/// </summary>
		[TestMethod]
		[DeploymentItem("BusinessLogic.dll")]
		public void ProjectStateManagerConstructorTest()
		{
		}
	}
}