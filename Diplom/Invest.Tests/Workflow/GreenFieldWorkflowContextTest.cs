using Invest.Common.Repository;
using Invest.Workflow.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Invest.Common.Model;
using Invest.Workflow.StateManagment;

namespace Invest.Tests
{
    
    
    /// <summary>
    ///This is a test class for GreenFieldWorkflowContextTest and is intended
    ///to contain all GreenFieldWorkflowContextTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GreenFieldWorkflowContextTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GreenFieldWorkflowContext Constructor
        ///</summary>
        [TestMethod()]
        public void GreenFieldWorkflowContextConstructorTest()
        {
            IRepository repository = null; // TODO: Initialize to an appropriate value
            GreenFieldWorkflowContext target = new GreenFieldWorkflowContext(repository);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateWorkflow
        ///</summary>
        [TestMethod()]
        public void CreateWorkflowTest()
        {
            IRepository repository = null; // TODO: Initialize to an appropriate value
            GreenFieldWorkflowContext target = new GreenFieldWorkflowContext(repository); // TODO: Initialize to an appropriate value
            IWorkflow expected = null; // TODO: Initialize to an appropriate value
            IWorkflow actual;
            
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetWorkflow
        ///</summary>
        [TestMethod()]
        public void GetWorkflowTest()
        {
            IRepository repository = null; // TODO: Initialize to an appropriate value
            GreenFieldWorkflowContext target = new GreenFieldWorkflowContext(repository); // TODO: Initialize to an appropriate value
            string id = string.Empty; // TODO: Initialize to an appropriate value
            IWorkflow expected = null; // TODO: Initialize to an appropriate value
            IWorkflow actual;
            actual = target.GetWorkflow(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SaveState
        ///</summary>
        [TestMethod()]
        public void SaveStateTest()
        {
            IRepository repository = null; // TODO: Initialize to an appropriate value
            GreenFieldWorkflowContext target = new GreenFieldWorkflowContext(repository); // TODO: Initialize to an appropriate value
            IWorkflow workflow = null; // TODO: Initialize to an appropriate value
            target.SaveState(workflow);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
