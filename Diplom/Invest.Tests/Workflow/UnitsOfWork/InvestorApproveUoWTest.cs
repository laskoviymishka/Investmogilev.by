using System.Collections;
using System.Collections.Generic;
using BusinessLogic.Notification;
using BusinessLogic.Wokflow.UnitsOfWork.Realization;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;

namespace Invest.Tests.Workflow.UnitsOfWork
{
    /// <summary>
    ///This is a test class for InvestorApproveUoWTest and is intended
    ///to contain all InvestorApproveUoWTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InvestorApproveUoWTest
    {
        #region Private Fields

        private IList _projects;
        private Project _currentProject;
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
                    History = new List<History>(),
                    CurrentState = ProjectWorkflow.State.Open
                }
            };
            _projects = new List<Project> { _currentProject };
            _userNotification = new Mock<IUserNotification>();
            _adminNotification = new Mock<IAdminNotification>();
            _investorNotification = new Mock<IInvestorNotification>();
            _userName = "test";
            _roles = new List<string>();
            _repository = new MockMongoRepository(_projects);
        }

        private InvestorApproveUoW CreateUoW()
        {
            return new InvestorApproveUoW(_currentProject,
                _repository,
                _userNotification.Object,
                _adminNotification.Object,
                _investorNotification.Object,
                _userName,
                _roles);
        }

        #endregion

        /// <summary>
        ///A test for InvestorApproveUoW Constructor
        ///</summary>
        [TestMethod()]
        public void InvestorApproveUoWConstructorTest()
        {
            InvestorApproveUoW target = CreateUoW();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for FromInvestorApproveToDocument
        ///</summary>
        [TestMethod()]
        public void FromInvestorApproveToDocumentTest()
        {
            _currentProject.Responses = new List<InvestorResponse>();
            InvestorApproveUoW target = CreateUoW();
            Assert.IsFalse(target.FromInvestorApproveToDocument());
            _currentProject.Responses = new List<InvestorResponse>();
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = false });
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = false });
            target = CreateUoW();
            Assert.IsFalse(target.FromInvestorApproveToDocument());
            _currentProject.Responses = new List<InvestorResponse>();
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = true });
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = true });
            target = CreateUoW();
            Assert.IsFalse(target.FromInvestorApproveToDocument());
            _currentProject.Responses = new List<InvestorResponse>();
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = false });
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = true });
            target = CreateUoW();
            Assert.IsTrue(target.FromInvestorApproveToDocument());
        }

        /// <summary>
        ///A test for FromInvestorApproveToInvestorResponsed
        ///</summary>
        [TestMethod()]
        public void FromInvestorApproveToInvestorResponsedTest()
        {
            InvestorApproveUoW target = CreateUoW();

            Assert.IsFalse(target.FromInvestorApproveToInvestorResponsed());

            _currentProject.Responses = new List<InvestorResponse>();
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = false });
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = false });
            target = CreateUoW();
            Assert.IsTrue(target.FromInvestorApproveToInvestorResponsed());

            _currentProject.Responses = new List<InvestorResponse>();
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = false });
            _currentProject.Responses.Add(new InvestorResponse() { IsVerified = true });
            target = CreateUoW();
            Assert.IsFalse(target.FromInvestorApproveToInvestorResponsed());
        }

        /// <summary>
        ///A test for FromOnMapToInvestorApprove
        ///</summary>
        [TestMethod()]
        public void FromOnMapToInvestorApproveTest()
        {
            InvestorApproveUoW target = CreateUoW();
            Assert.IsTrue(target.FromOnMapToInvestorApprove());
        }

        /// <summary> 
        ///A test for  OnInvestorApproveExit
        ///</summary>
        [TestMethod()]
        public void OnInvestorApproveExitTest()
        {
        }

        /// <summary>
        ///A test for OnInvestorApproveEntry
        ///</summary>
        [TestMethod()]
        public void OnInvestorApproveEntryTest()
        {
            bool wasUserNotificate = false;
            bool wasAdminNotificate = false;

            _adminNotification.Setup(a => a.InvestorResponsed(It.IsAny<Project>())).Callback(() =>
                { wasAdminNotificate = true; });
            _userNotification.Setup(u => u.InvestorResponsed(It.IsAny<Project>())).Callback(() =>
                { wasUserNotificate = true; });

            _currentProject.WorkflowState.CurrentState = ProjectWorkflow.State.OnMap;
            _currentProject.Responses = new List<InvestorResponse>
                {
                    new InvestorResponse() {IsVerified = false},
                    new InvestorResponse() {IsVerified = false},
                    new InvestorResponse() {IsVerified = true}
                };

            var target = CreateUoW();
            target.OnInvestorApproveEntry();

            Assert.IsTrue(wasAdminNotificate);
            Assert.IsTrue(wasUserNotificate);
            Assert.IsTrue(
                _repository.GetOne<Project>(
                    p => p._id == _currentProject._id).WorkflowState.CurrentState == ProjectWorkflow.State.InvestorApprove);

            Assert.IsTrue(_repository.GetOne<Project>(
                    p => p._id == _currentProject._id).WorkflowState.History.Count > 0);

            Assert.IsTrue(_repository.GetOne<Project>(
                    p => p._id == _currentProject._id)
                        .WorkflowState.History.Find(
                            h =>
                                h.Editor == _userName
                                && h.From == ProjectWorkflow.State.OnMap
                                && h.To == ProjectWorkflow.State.InvestorApprove) != null);
        }

    }
}
