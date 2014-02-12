namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
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