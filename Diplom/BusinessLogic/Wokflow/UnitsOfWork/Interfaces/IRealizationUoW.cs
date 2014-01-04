namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IRealizationUoW
    {

        void OnRealizationExit();

        void OnRealizationEntry();

        bool CouldUpdateRealization();

        bool CouldUpdateRealizationAndLeave();
    }
}