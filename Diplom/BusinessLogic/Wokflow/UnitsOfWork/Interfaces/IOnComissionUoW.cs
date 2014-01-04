namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IOnComissionUoW
    {

        void OnOnComissionExit();

        void OnOnComissionEntry();

        bool CouldComissionFix();

        bool CouldToIspolcom();
    }
}