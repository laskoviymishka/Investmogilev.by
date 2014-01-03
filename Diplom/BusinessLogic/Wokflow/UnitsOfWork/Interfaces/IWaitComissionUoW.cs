namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IWaitComissionUoW
    {

        void OnWaitComissionExit();

        void OnWaitComissionEntry();

        bool CouldComission();
    }
}