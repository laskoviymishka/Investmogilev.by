namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IInvestorApproveUoW
    {
        void OnInvestorApproveExit();
        void OnInvestorApproveEntry();

        bool FromInvestorApproveToDocument();
        bool FromInvestorApproveToInvestorResponsed();
        bool FromOnMapToInvestorApprove();
    }
}