namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IOpenUoW
    {
        void OnOpenExit();
        void OnOpenEntry();

        bool FromMapToOpen();
        bool FromOpenToMap();
    }
}