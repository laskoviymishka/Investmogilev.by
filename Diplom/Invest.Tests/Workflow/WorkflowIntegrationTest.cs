using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic.Wokflow;
using Invest.Common.State;
using Moq;
using Invest.Common.Notification;
using Invest.Common.Model.Project;
using System.Collections;
using BusinessLogic.Notification;
using MongoDB.Bson;

namespace Invest.Tests.Workflow
{
    /// <summary>
    /// Summary description for WorkflowIntegrationTest
    /// </summary>
    [TestClass]
    public class WorkflowIntegrationTest
    {
        #region Private Fields

        private Project _currentProject;
        private IList _projects;
        private MockMongoRepository _repository;
        private Mock<IUserNotification> _userNotification;
        private Mock<IAdminNotification> _adminNotification;
        private Mock<IInvestorNotification> _investorNotification;
        private string _userName;
        private IEnumerable<string> _roles;
        private ProjectWorkflowWrapper _workflow;
        private UnitsOfWorkContainer _unitOfWorksContainer;

        #endregion

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
                    History = new List<History>(),
                    CurrentState = ProjectWorkflow.State.Open
                }
            };
            _projects = new List<Project> { _currentProject };

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
            _workflow = new ProjectWorkflowWrapper(
                new ProjectWorkflow(ProjectWorkflow.State.Open),
                _currentProject,
                _investorNotification.Object,
                _adminNotification.Object,
                _userNotification.Object,
                _unitOfWorksContainer);


        }

        [TestMethod]
        public void OpenToMapTransition()
        {
            _roles = new List<string>() { "User" };
            MyTestInitialize();
            _workflow.TryMove(ProjectWorkflow.Trigger.FillInformation);
            Assert.IsTrue(_repository.GetOne<Project>(p => p._id == _currentProject._id).WorkflowState.CurrentState == ProjectWorkflow.State.OnMap);
        }
    }
}
