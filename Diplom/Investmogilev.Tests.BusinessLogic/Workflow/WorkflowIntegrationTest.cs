using System.Collections;
using System.Collections.Generic;
using Investmogilev.Infrastructure.BusinessLogic.Notification;
using Investmogilev.Infrastructure.BusinessLogic.Wokflow;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;

namespace Investmogilev.Tests.BusinessLogic.Workflow
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
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "testProjectName",
                Region = "testProjectRegion",
                InvestorUser = "",
                Address = new Address { Lat = 53, Lng = 30 },
                WorkflowState = new Investmogilev.Infrastructure.Common.Model.Project.Workflow
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
        }

        private ProjectWorkflowWrapper Wrapper()
        {
            return new ProjectWorkflowWrapper(
                new ProjectWorkflow(ProjectWorkflow.State.Open),
                _unitOfWorksContainer);
        }


        [TestMethod]
        public void OpenToMapTransition()
        {
            _roles = new List<string>() { "User" };
            MyTestInitialize();
            _workflow = Wrapper();
            _workflow.Move(ProjectWorkflow.Trigger.FillInformation);
            Assert.IsTrue(_repository.GetOne<Project>(p => p.Id == _currentProject.Id).WorkflowState.CurrentState == ProjectWorkflow.State.OnMap);
        }

        [TestMethod]
        public void ReOpnFromMapTransition()
        {
            _roles = new List<string>() { "User" };
            MyTestInitialize();
            _workflow = Wrapper();
            _workflow.Move(ProjectWorkflow.Trigger.FillInformation);
            Assert.IsTrue(_repository.GetOne<Project>(p => p.Id == _currentProject.Id).WorkflowState.CurrentState == ProjectWorkflow.State.OnMap);

            _roles = new List<string>() { "Admin" };
            MyTestInitialize();
            _workflow.Move(ProjectWorkflow.Trigger.ReOpen);
            Assert.IsTrue(_repository.GetOne<Project>(p => p.Id == _currentProject.Id).WorkflowState.CurrentState == ProjectWorkflow.State.Open);
        }
    }
}
