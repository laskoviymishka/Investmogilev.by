namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IWaitIspolcomUoW
    {

        void OnWaitIspolcomExit();

        void OnWaitIspolcomEntry();

        bool CouldToIspolcom();
    }
}