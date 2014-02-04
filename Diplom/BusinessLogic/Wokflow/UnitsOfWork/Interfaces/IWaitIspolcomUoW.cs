namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
    public interface IWaitIspolcomUoW
    {

        void OnWaitIspolcomExit();

        void OnWaitIspolcomEntry();

        bool CouldToIspolcom();
    }
}