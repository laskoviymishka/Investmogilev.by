namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
    public interface IRealizationUoW
    {

        void OnRealizationExit();

        void OnRealizationEntry();

        bool CouldUpdateRealization();

        bool CouldUpdateRealizationAndLeave();
    }
}