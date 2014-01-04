namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IDoneUoW
    {

        void OnDoneExit();

        void OnDoneEntry();
    }
}