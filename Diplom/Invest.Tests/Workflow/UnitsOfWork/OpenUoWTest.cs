using System.Collections;
using System.Collections.Generic;
using BusinessLogic.Notification;
using BusinessLogic.Wokflow.UnitsOfWork;
using Invest.Common.Model.Project;
using Invest.Common.Notification;
using Invest.Common.Repository;
using Invest.Common.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;

namespace Invest.Tests.Workflow.UnitsOfWork
{
    /// <summary>
    ///This is a test class for OpenUoWTest and is intended
    ///to contain all OpenUoWTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OpenUoWTest
    {
        #region Private Fields

        private Project _currentProject;
        private IList _projects;
        private IRepository _repository;
        private Mock<IUserNotification> _userNotification;
        private Mock<IAdminNotification> _adminNotification;
        private Mock<IInvestorNotification> _investorNotification;
        private string _userName;
        private IEnumerable<string> _roles;
        #endregion

        #region Initialize

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _currentProject = new Project
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    Name = "testProjectName",
                    Region = "testProjectRegion",
                    InvestorUser = "",
                    Address = new Address { Lat = 52, Lng = 53 },
                    WorkflowState = new Common.Model.Project.Workflow
                        {
                            History = new History(),
                            CurrentState = ProjectWorkflow.State.Open
                        }
                };
            _projects = new List<Project> { _currentProject };

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
        ///A test for OpenUoW Constructor
        ///</summary>
        [TestMethod()]
        public void OpenUoWConstructorTest()
        {
            var target = CreateUoW();
            Assert.IsNotNull(_repository.GetOne<Project>(t => t._id == _currentProject._id));
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for FromMapToOpen
        ///</summary>
        [TestMethod()]
        public void FromMapToOpenTest()
        {
            var target = CreateUoW();
            _currentProject.WorkflowState.CurrentState = ProjectWorkflow.State.OnMap;
            Assert.IsNotNull(_repository.GetOne<Project>(t => t._id == _currentProject._id));
            Assert.IsTrue(_repository.GetOne<Project>(t => t._id == _currentProject._id).WorkflowState.CurrentState == ProjectWorkflow.State.OnMap);
            Assert.IsFalse(target.FromMapToOpen());
            _roles = new string[] { "Admin" };
            target = CreateUoW();
            Assert.IsTrue(target.FromMapToOpen());
        }

        /// <summary>
        ///A test for FromOpenToMap
        ///</summary>
        [TestMethod()]
        public void FromOpenToMapTest()
        {
            var target = CreateUoW();
            _currentProject.WorkflowState.CurrentState = ProjectWorkflow.State.Open;
            Assert.IsNotNull(_repository.GetOne<Project>(t => t._id == _currentProject._id));
            Assert.IsTrue(_repository.GetOne<Project>(t => t._id == _currentProject._id).WorkflowState.CurrentState == ProjectWorkflow.State.Open);
            Assert.IsFalse(target.FromOpenToMap());
            _roles = new string[] { "User" };
            target = CreateUoW();
            Assert.IsTrue(target.FromOpenToMap());
        }

        /// <summary>
        ///A test for OnOpenEntry
        ///</summary>
        [TestMethod()]
        public void OnOpenEntryTest()
        {
            bool wasNotificated = false;
            var target = CreateUoW();
            _adminNotification.Setup(a => a.NotificateFill()).Callback(() => { wasNotificated = true; });
            target.OnOpenExit();
            Assert.IsTrue(wasNotificated);
        }

        /// <summary>
        ///A test for OnOpenExit
        ///</summary>
        [TestMethod()]
        public void OnOpenExitTest()
        {
            bool wasNotificated = false;
            var target = CreateUoW();
            _userNotification.Setup(a => a.NotificateOpen()).Callback(() => { wasNotificated = true; });
            target.OnOpenEntry();
            Assert.IsTrue(wasNotificated);
        }
    }
}
