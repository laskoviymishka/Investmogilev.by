namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IOnIspolcomUoW
	{
		void OnOnIspolcomExit();

		void OnOnIspolcomEntry();

		bool CouldToMinEconomy();

		bool CouldToIspolcomFix();
	}
}