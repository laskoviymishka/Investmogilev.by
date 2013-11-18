namespace Invest.Workflow.StateManagment
{
    public interface IWorkflowContext
    {
        IWorkflow GetWorkflow(string id);
        IWorkflow CreateWorkflow();
        void SaveState(IWorkflow workflow); 
    }
}