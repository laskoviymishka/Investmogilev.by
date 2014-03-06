using System;
using System.Collections;
using System.Collections.Generic;
using Investmogilev.Infrastructure.BusinessLogic.Notification;
using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Realization;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;

namespace Investmogilev.Tests.BusinessLogic.Workflow.UnitsOfWork
{
	/// <summary>
	///     This is a test class for OnMapUoWTest and is intended
	///     to contain all OnMapUoWTest Unit Tests
	/// </summary>
	[TestClass]
	public class OnMapUoWTest
	{
		#region Private Fields

		private Mock<IAdminNotification> _adminNotification;
		private Project _currentProject;
		private Mock<IInvestorNotification> _investorNotification;
		private IList _projects;
		private MockMongoRepository _repository;
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
				WorkflowState = new Investmogilev.Infrastructure.Common.Model.Project.Workflow
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

		private OnMapUoW CreateUoW()
		{
			return new OnMapUoW(_currentProject,
				_repository,
				_userNotification.Object,
				_adminNotification.Object,
				_investorNotification.Object,
				_userName,
				_roles);
		}

		#endregion

		/// <summary>
		///     A test for FromOnComissionToOnMap
		/// </summary>
		[TestMethod]
		public void FromOnComissionToOnMapTest()
		{
			OnMapUoW target = CreateUoW();

			Assert.IsFalse(target.FromOnComissionToOnMap());
			_roles = new[] {"Admin"};
			target = CreateUoW();
			Assert.IsTrue(target.FromOnComissionToOnMap());
		}

		/// <summary>
		///     A test for OnMapUoW Constructor
		/// </summary>
		[TestMethod]
		public void OnMapUoWConstructorTest()
		{
			OnMapUoW target = CreateUoW();
			Assert.IsNotNull(target);
		}

		/// <summary>
		///     A test for FromOnIspolcomToOnMap
		/// </summary>
		[TestMethod]
		public void FromOnIspolcomToOnMapTest()
		{
			OnMapUoW target = CreateUoW();

			Assert.IsFalse(target.FromOnIspolcomToOnMap());
			_roles = new[] {"Admin"};
			target = CreateUoW();
			Assert.IsTrue(target.FromOnIspolcomToOnMap());
		}

		/// <summary>
		///     A test for FromOnMapToInvestorApprove
		/// </summary>
		[TestMethod]
		public void FromOnMapToInvestorApproveTest()
		{
			OnMapUoW target = CreateUoW();

			Assert.IsTrue(target.FromOnMapToInvestorApprove());
			_currentProject.Responses = new List<InvestorResponse> {new InvestorResponse()};
			target = CreateUoW();
			Assert.IsTrue(target.FromOnMapToInvestorApprove());
		}

		/// <summary>
		///     A test for FromOnMapToOnMap
		/// </summary>
		[TestMethod]
		public void FromOnMapToOnMapTest()
		{
			OnMapUoW target = CreateUoW();

			Assert.IsFalse(target.FromOnMapToOnMap());
			_roles = new[] {"User"};
			target = CreateUoW();
			Assert.IsTrue(target.FromOnMapToOnMap());
			_roles = new[] {"Admin"};
			target = CreateUoW();
			Assert.IsTrue(target.FromOnMapToOnMap());
		}

		/// <summary>
		///     A test for FromOnMapToOpen
		/// </summary>
		[TestMethod]
		public void FromOnMapToOpenTest()
		{
			OnMapUoW target = CreateUoW();

			Assert.IsFalse(target.FromOnMapToOpen());
			_roles = new[] {"Admin"};
			target = CreateUoW();
			Assert.IsTrue(target.FromOnMapToOpen());
		}

		/// <summary>
		///     A test for FromWaitComissionFixesToOnMap
		/// </summary>
		[TestMethod]
		public void FromWaitComissionFixesToOnMapTest()
		{
			OnMapUoW target = CreateUoW();

			Assert.IsFalse(target.FromWaitComissionFixesToOnMap());
			_roles = new[] {"Admin"};
			target = CreateUoW();
			Assert.IsTrue(target.FromWaitComissionFixesToOnMap());
		}

		/// <summary>
		///     A test for FromWaitIspolcomFixesToOnMap
		/// </summary>
		[TestMethod]
		public void FromWaitIspolcomFixesToOnMapTest()
		{
			OnMapUoW target = CreateUoW();

			Assert.IsFalse(target.FromWaitIspolcomFixesToOnMap());
			_roles = new[] {"Admin"};
			target = CreateUoW();
			Assert.IsTrue(target.FromWaitIspolcomFixesToOnMap());
		}

		/// <summary>
		///     A test for OnMapEntry
		/// </summary>
		[TestMethod]
		public void OnMapEntryTest()
		{
			bool wasExceptions = false;
			bool wasExceptions2 = false;
			bool wasNotificated = false;
			_adminNotification.Setup(a => a.MapEntryNotificate()).Callback(() => { wasNotificated = true; });

			_currentProject.WorkflowState.CurrentState = ProjectWorkflow.State.OnMap;
			OnMapUoW target = CreateUoW();
			try
			{
				target.OnMapEntry();
			}
			catch (InvalidOperationException e)
			{
				wasExceptions = true;
			}

			_currentProject.Address = new Address {Lat = 2, Lng = 3};
			target = CreateUoW();
			try
			{
				target.OnMapEntry();
			}
			catch (InvalidOperationException e)
			{
				wasExceptions2 = true;
			}

			_currentProject.Address = new Address {Lat = 54.2807765015195, Lng = 30.9715922176838};
			_currentProject.WorkflowState.CurrentState = ProjectWorkflow.State.Open;
			target = CreateUoW();

			target.OnMapEntry();

			Assert.IsTrue(wasExceptions);
			Assert.IsTrue(wasExceptions2);
			Assert.IsTrue(
				_repository.GetOne<Project>(
					p => p._id == _currentProject._id).WorkflowState.CurrentState == ProjectWorkflow.State.OnMap);

			Assert.IsTrue(_repository.GetOne<Project>(
				p => p._id == _currentProject._id).WorkflowState.History.Count > 0);

			Assert.IsTrue(_repository.GetOne<Project>(
				p => p._id == _currentProject._id)
				.WorkflowState.History.Find(
					h =>
						h.Editor == _userName
						&& h.From == ProjectWorkflow.State.Open
						&& h.To == ProjectWorkflow.State.OnMap) != null);

			Assert.IsTrue(wasNotificated);
		}

		/// <summary>
		///     A test for OnMapExit
		/// </summary>
		[TestMethod]
		public void OnMapExitTest()
		{
		}
	}
}