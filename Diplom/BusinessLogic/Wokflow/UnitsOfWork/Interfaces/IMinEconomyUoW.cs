namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IMinEconomyUoW
    {

        void OnInMinEconomyExit();

        void OnInMinEconomyEntry();

        bool CouldMinEconomyResponsed();
    }
}