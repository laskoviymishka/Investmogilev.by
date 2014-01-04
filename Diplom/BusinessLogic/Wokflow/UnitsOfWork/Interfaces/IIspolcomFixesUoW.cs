namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IIspolcomFixesUoW
    {

        void OnWaitIspolcomFixesExit();

        void OnWaitIspolcomFixesEntry();

        bool CouldIspolcomFixUpdate();

        bool CouldIspolcomFixUpdateAndLeave();
    }
}