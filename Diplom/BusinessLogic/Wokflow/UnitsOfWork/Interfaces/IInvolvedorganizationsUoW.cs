namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IInvolvedorganizationsUoW
    {
        void OnInvolvedOrganizationsExit();
        void OnInvolvedOrganizationsEntry();
        bool CouldInvolvedOrganizationUpdate();
        bool CouldInvolvedOrganizationUpdateAndLeave();
    }
}