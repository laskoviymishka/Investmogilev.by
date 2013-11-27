using Invest.Common.Model.ProjectModels;
using Invest.Workflow.StateManagment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Invest.Common.Model;
using System.Collections.Generic;

namespace Invest.Tests
{
    /// <summary>
    ///This is a test class for BaseWorkflowTest and is intended
    ///to contain all BaseWorkflowTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BaseWorkflowTest
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
        ///A test for BaseWorkflow`1 Constructor
        ///</summary>
        public void BaseWorkflowConstructorTestHelper<T>()
            where T : Project
        {
            string initialState = string.Empty; // TODO: Initialize to an appropriate value
            BaseWorkflow<T> target = new BaseWorkflow<T>(initialState);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        [TestMethod()]
        public void BaseWorkflowConstructorTest()
        {
            Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of T. " +
                    "Please call BaseWorkflowConstructorTestHelper<T>() with appropriate type paramet" +
                    "ers.");
        }

        /// <summary>
        ///A test for Move
        ///</summary>
        public void MoveTestHelper<T>()
            where T : Project
        {
            string initialState = string.Empty; // TODO: Initialize to an appropriate value
            BaseWorkflow<T> target = new BaseWorkflow<T>(initialState); // TODO: Initialize to an appropriate value
            string from = string.Empty; // TODO: Initialize to an appropriate value
            string to = string.Empty; // TODO: Initialize to an appropriate value
            string editor = string.Empty; // TODO: Initialize to an appropriate value
            Dictionary<string, object> conditions = null; // TODO: Initialize to an appropriate value
            target.Move(from, to, editor, conditions);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void MoveTest()
        {
            Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of T. " +
                    "Please call MoveTestHelper<T>() with appropriate type parameters.");
        }

        /// <summary>
        ///A test for SetContext
        ///</summary>
        public void SetContextTestHelper<T>()
            where T : Project
        {
            string initialState = string.Empty; // TODO: Initialize to an appropriate value
            BaseWorkflow<T> target = new BaseWorkflow<T>(initialState); // TODO: Initialize to an appropriate value
            IWorkflowContext context = null; // TODO: Initialize to an appropriate value
            target.SetContext(context);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void SetContextTest()
        {
            Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of T. " +
                    "Please call SetContextTestHelper<T>() with appropriate type parameters.");
        }

        /// <summary>
        ///A test for Workflow
        ///</summary>
        public void WorkflowTestHelper<T>()
            where T : Project
        {
            string initialState = string.Empty; // TODO: Initialize to an appropriate value
            BaseWorkflow<T> target = new BaseWorkflow<T>(initialState); // TODO: Initialize to an appropriate value
            WorkflowEntity expected = null; // TODO: Initialize to an appropriate value
            WorkflowEntity actual;
            target.Workflow = expected;
            actual = target.Workflow;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void WorkflowTest()
        {
            Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of T. " +
                    "Please call WorkflowTestHelper<T>() with appropriate type parameters.");
        }

        /// <summary>
        ///A test for CurrenState
        ///</summary>
        public void CurrenStateTestHelper<T>()
            where T : Project
        {
            string initialState = string.Empty; // TODO: Initialize to an appropriate value
            BaseWorkflow<T> target = new BaseWorkflow<T>(initialState); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CurrenState = expected;
            actual = target.CurrenState;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void CurrenStateTest()
        {
            Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of T. " +
                    "Please call CurrenStateTestHelper<T>() with appropriate type parameters.");
        }
    }
}
