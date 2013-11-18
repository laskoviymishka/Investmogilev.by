using Invest.Workflow.StateManagment;
using MongoRepository.Repository;
using Invest.Common.Model;

namespace Invest.Workflow.User
{
    public class UserWorkflowContext : IWorkflowContext
    {
        #region Private Fields

        private readonly IRepository<MongoUser> _repository; 

        #endregion

        #region Constructor

        public UserWorkflowContext(IRepository<MongoUser> repository)
        {
            _repository = repository;
        }

        #endregion

        #region IWorkflowContext implementation

        public IWorkflow GetWorkflow(string id)
        {
            _repository.GetById(id);
            return null;
        }

        public IWorkflow CreateWorkflow()
        {
            throw new System.NotImplementedException();
        }

        public void SaveState(IWorkflow workflow)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}