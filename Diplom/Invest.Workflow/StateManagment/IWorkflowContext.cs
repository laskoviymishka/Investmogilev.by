namespace Invest.Workflow.StateManagment
{
    public interface IWorkflowContext
    {
        IWorkflow GetWorkflow(string id);
        IWorkflow CreateWorkflow(string workflowForLink);
        void SaveState(IWorkflow workflow); 
    }
}