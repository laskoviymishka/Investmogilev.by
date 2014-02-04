namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
    public interface IOpenUoW
    {
        void OnOpenExit();
        void OnOpenEntry();

        bool FromMapToOpen();
        bool FromOpenToMap();
    }
}