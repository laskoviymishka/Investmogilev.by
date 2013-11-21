using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Invest.Common.Model;
using Invest.Common.Repository;
using Invest.Workflow.StateManagment;
using History = Invest.Common.Model.History;

namespace Invest.Workflow.Project
{
    public class GreenFieldWorkflowContext : GreenFieldStates, IWorkflowContext
    {
        private readonly IRepository _repository;

        public GreenFieldWorkflowContext(IRepository repository)
        {
            _repository = repository;
        }

        public IWorkflow GetWorkflow(string id)
        {
            var workflowEntity = _repository.GetOne<WorkflowEntity>(w => w._id == id);
            IWorkflow workflow = new BaseWorkflow<GreenField>(workflowEntity.CurrenState);
            var conditions =
                new Dictionary<string, Func<object, bool>>();
            conditions["Role"] = (o) => o is string && o.ToString() == "Admin";

            workflow.Transitions = new List<ITransition>()
                {
                    new BaseTransition(
                        Open,
                        UnVerifyDone,
                        conditions,
                        new List<Func<bool>>()
                            {
                                TestTransition
                            }),
                    new BaseTransition(
                        UnVerifyDone,
                        Progress,
                        conditions,
                        new List<Func<bool>>()
                            {
                                TestTransition
                            }),
                    new BaseTransition(
                        Progress,
                        PendingPlanChanged,
                        conditions,
                        new List<Func<bool>>()
                            {
                                TestTransition
                            }),
                    new BaseTransition(
                        PendingPlanChanged,
                        Close,
                        conditions,
                        new List<Func<bool>>()
                            {
                                TestTransition
                            })
                };
            workflow.CurrentCondiotions = new Dictionary<string, object>();
            workflow.Workflow = workflowEntity;
            workflow.SetContext(this);
            return workflow;
        }

        private bool TestTransition()
        {
            return true;
        }

        public IWorkflow CreateWorkflow(string projectId)
        {
            IWorkflow workflow = new BaseWorkflow<GreenField>(Open);
            var conditions =
                new Dictionary<string, Func<object, bool>>();
            conditions["Role"] = (o) => o is string && o.ToString() == "Admin";

            workflow.Transitions = new List<ITransition>()
                {
                    new BaseTransition(
                        Open,
                        UnVerifyDone,
                        conditions,
                        new List<Func<bool>>()
                            {
                                TestTransition
                            }),
                    new BaseTransition(
                        UnVerifyDone,
                        Progress,
                        conditions,
                        new List<Func<bool>>()
                            {
                                TestTransition
                            }),
                    new BaseTransition(
                        Progress,
                        PendingPlanChanged,
                        conditions,
                        new List<Func<bool>>()
                            {
                                TestTransition
                            }),
                    new BaseTransition(
                        PendingPlanChanged,
                        Close,
                        conditions,
                        new List<Func<bool>>()
                            {
                                TestTransition
                            })
                };
            workflow.Workflow = new WorkflowEntity();
            workflow.CurrentCondiotions = new Dictionary<string, object>();
            workflow.CurrentCondiotions["Role"] = "Admin";
            workflow.Workflow.ChangeHistory = new List<History>();
            workflow.Workflow.CurrenState = Open;
            workflow.Workflow.ProjectId = projectId;
            _repository.Add(workflow.Workflow);
            workflow.SetContext(this);
            return workflow;
        }

        public void SaveState(IWorkflow workflow)
        {
            _repository.Update(workflow.Workflow);
        }
    }
}
