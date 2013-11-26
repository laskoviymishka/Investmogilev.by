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
    public class GreenFieldWorkflowContext : IWorkflowContext
    {
        private readonly IRepository _repository;
        private IWorkflow _workflow;

        public GreenFieldWorkflowContext(IRepository repository)
        {
            _repository = repository;
        }

        public IWorkflow GetWorkflow(string projectId)
        {
            //var workflowEntity = _repository.GetOne<Invest.Common.Model.Project>(w => w._id == projectId).WorkflowState;
            //if (workflowEntity == null)
            //{
            //    return CreateWorkflow(projectId);
            //}
            //_workflow = new BaseWorkflow<GreenField>(workflowEntity.CurrenState);


            //_workflow.Transitions = GetTransitions();
            //_workflow.CurrentCondiotions = new Dictionary<string, object>();
            //_workflow.Workflow = workflowEntity;
            //_workflow.SetContext(this);

            //return _workflow;
            return null;
        }

        private List<ITransition> GetTransitions()
        {
            List<ITransition> transition = new List<ITransition>();

            transition.Add(FromOpenToWaitForApproveResponse);
            return transition;
        }

        private static ITransition FromOpenToWaitForApproveResponse
        {
            get
            {
                var fromOpenToWaitForApproveResponseConditions = new Dictionary<string, Func<object, bool>>();

                var actions = new List<Func<bool>>();

                ITransition fromOpenToWaitForApproveResponse =
                    new BaseTransition(
                        GreenFieldStates.Open,
                        GreenFieldStates.WaitForVerifyResponse,
                        fromOpenToWaitForApproveResponseConditions,
                        actions);
                return fromOpenToWaitForApproveResponse;
            }
        }

        private bool CheckCurrentState()
        {
            return true;
        }

        public IWorkflow CreateWorkflow(string projectId)
        {
            _workflow = new BaseWorkflow<GreenField>(GreenFieldStates.Open);

            _workflow.Transitions = GetTransitions();
            _workflow.Workflow = new WorkflowEntity();
            _workflow.Workflow.ChangeHistory = new List<History>();
            _workflow.Workflow.CurrenState = GreenFieldStates.Open;
            _workflow.SetContext(this);
            return _workflow;
        }

        public void SaveState(IWorkflow workflow)
        {
        }
    }
}
