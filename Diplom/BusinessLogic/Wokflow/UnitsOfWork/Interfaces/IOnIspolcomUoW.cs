namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IOnIspolcomUoW
    {

        void OnOnIspolcomExit();

        void OnOnIspolcomEntry();

        bool CouldToMinEconomy();

        bool CouldToIspolcomFix();
    }
}