using System.Collections.Generic;
using BusinessLogic.Notification;
using BusinessLogic.Wokflow.UnitsOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.Notification;
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

        private Project _currentProject;
        private Mock<IRepository> _repository;
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
            _currentProject = new Project();
            _repository = new Mock<IRepository>();
            _userNotification = new Mock<IUserNotification>();
            _adminNotification = new Mock<IAdminNotification>();
            _investorNotification = new Mock<IInvestorNotification>();
            _userName = "test";
            _roles = new List<string>();
        }

        private IInvestorApproveUoW CreateUoW()
        {
            return new InvestorApproveUoW(_currentProject,
                _repository.Object,
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
            IInvestorApproveUoW target = CreateUoW();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for FromInvestorApproveToDocument
        ///</summary>
        [TestMethod()]
        public void FromInvestorApproveToDocumentTest()
        {
            IInvestorApproveUoW target = CreateUoW();
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FromInvestorApproveToInvestorResponsed
        ///</summary>
        [TestMethod()]
        public void FromInvestorApproveToInvestorResponsedTest()
        {
            IInvestorApproveUoW target = CreateUoW();
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FromOnMapToInvestorApprove
        ///</summary>
        [TestMethod()]
        public void FromOnMapToInvestorApproveTest()
        {
            IInvestorApproveUoW target = CreateUoW();
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OnInvestorApproveEntry
        ///</summary>
        [TestMethod()]
        public void OnInvestorApproveEntryTest()
        {
            IInvestorApproveUoW target = CreateUoW();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnInvestorApproveExit
        ///</summary>
        [TestMethod()]
        public void OnInvestorApproveExitTest()
        {
            IInvestorApproveUoW target = CreateUoW();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
