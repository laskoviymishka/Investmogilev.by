namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IMinEconomyUoW
	{
		void OnInMinEconomyExit();

		void OnInMinEconomyEntry();

		bool CouldMinEconomyResponsed();
	}
}