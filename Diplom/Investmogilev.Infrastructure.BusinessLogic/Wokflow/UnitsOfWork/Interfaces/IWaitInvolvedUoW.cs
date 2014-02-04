namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
    public interface IWaitInvolvedUoW
    {

        void OnWaitInvolvedExit();

        void OnWaitInvolvedEntry();

        bool CouldFillInvolvedOrganization();
    }
}