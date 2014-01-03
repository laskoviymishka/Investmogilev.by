namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IWaitInvolvedUoW
    {

        void OnWaitInvolvedExit();

        void OnWaitInvolvedEntry();

        bool CouldFillInvolvedOrganization();
    }
}