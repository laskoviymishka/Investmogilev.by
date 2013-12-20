using System.Collections.Generic;
using BusinessLogic.Notification;
using BusinessLogic.Wokflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.Notification;
using Moq;

namespace Invest.Tests.Workflow.UnitsOfWork
{
    /// <summary>
    ///This is a test class for UnitsOfWorkContainerTest and is intended
    ///to contain all UnitsOfWorkContainerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UnitsOfWorkContainerTest
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

        private IUnitsOfWorkContainer CreateCintainer()
        {
            return new UnitsOfWorkContainer(_currentProject,
                _repository.Object,
                _userNotification.Object,
                _adminNotification.Object,
                _investorNotification.Object,
                _userName,
                _roles);
        }

        /// <summary>
        ///A test for UnitsOfWorkContainer Constructor
        ///</summary>
        [TestMethod()]
        public void UnitsOfWorkContainerConstructorTest()
        {
            IUnitsOfWorkContainer target = CreateCintainer();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for WaitIspolcomUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void WaitIspolcomUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.WaitIspolcomUoW);
        }

        /// <summary>
        ///A test for ComissionFixesUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void ComissionFixesUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.ComissionFixesUoW);
        }

        /// <summary>
        ///A test for DocumentSendingUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void DocumentSendingUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.DocumentSendingUoW);
        }

        /// <summary>
        ///A test for DoneUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void DoneUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.DoneUoW);
        }

        /// <summary>
        ///A test for InvestorApproveUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void InvestorApproveUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.InvestorApproveUoW);
        }

        /// <summary>
        ///A test for InvolvedorganizationsUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void InvolvedorganizationsUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.InvolvedorganizationsUoW);
        }

        /// <summary>
        ///A test for IspolcomFixesUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void IspolcomFixesUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.IspolcomFixesUoW);
        }

        /// <summary>
        ///A test for MinEconomyUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void MinEconomyUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.MinEconomyUoW);
        }

        /// <summary>
        ///A test for OnComissionUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void OnComissionUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.OnComissionUoW);
        }

        /// <summary>
        ///A test for OnIspolcomUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void OnIspolcomUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.OnIspolcomUoW);
        }

        /// <summary>
        ///A test for OnMapUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void OnMapUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.OnMapUoW);
        }

        /// <summary>
        ///A test for OpenUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void OpenUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.OpenUoW);
        }

        /// <summary>
        ///A test for PlanCreatingUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void PlanCreatingUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.PlanCreatingUoW);
        }

        /// <summary>
        ///A test for RealizationUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void RealizationUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.RealizationUoW);
        }

        /// <summary>
        ///A test for WaitComissionUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void WaitComissionUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.WaitComissionUoW);
        }

        /// <summary>
        ///A test for WaitInvolvedUoW
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BusinessLogic.dll")]
        public void WaitInvolvedUoWTest()
        {
            var container = CreateCintainer();
            Assert.IsNotNull(container.WaitInvolvedUoW);
        }
    }
}
