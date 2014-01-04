namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IPlanCreatingUoW
    {

        void OnPlanCreatingExit();

        void OnPlanCreatingEntry();

        bool CouldUpdatePlan();

        bool CouldApprovePlan();
    }
}