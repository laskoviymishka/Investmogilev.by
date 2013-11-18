using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Invest.Common.Model;
using Invest.Workflow.StateManagment;
using MongoRepository.Repository;

namespace Invest.Workflow.Project
{
    public class GreenFieldWorkflowContext : GreenFieldStates
    {
        private readonly IRepository<IWorkflow> _repository;

        public GreenFieldWorkflowContext(IRepository<IWorkflow> repository)
        {
            _repository = repository;
        }

        public IWorkflow GetWorkflow(string id)
        {
            var workflow = _repository.GetById(id);
            workflow.SetContext(this);
            return workflow;
        }

        public IWorkflow CreateWorkflow()
        {
            IWorkflow workflow = new BaseWorkflow<GreenField>(Open);
            var conditions =
                new Dictionary<string, Func<object, bool>>();
            conditions["Role"] = (o) => o is string && o.ToString() == "Admin";

            workflow.Transitions = new List<ITransition>()
                {
                    new BaseTransition(Open,UnVerifyDone, conditions, () =>
                        { return UnVerifyDone; }),
                    new BaseTransition(UnVerifyDone,Progress, conditions, () =>
                        { return Progress; }),
                    new BaseTransition(Progress,PendingPlanChanged, conditions, () =>
                        { return PendingPlanChanged; }),
                    new BaseTransition(PendingPlanChanged,Close, conditions, () =>
                        { return Close; })
                };
            workflow.CurrentCondiotions = new Dictionary<string, object>();
            workflow.CurrentCondiotions["Role"] = "Admin";
            _repository.Insert(workflow);
            return workflow;
        }

        public void SaveState(IWorkflow workflow)
        {
            _repository.Update(workflow);
        }
    }
}
