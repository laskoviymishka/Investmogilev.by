namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IComissionFixesUoW
    {

        void OnWaitComissionFixesExit();

        void OnWaitComissionFixesEntry();

        bool CouldUpdateComissionFix();

        bool CouldUpdateComissionFixAndLeave();
    }
}