namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IWaitComissionUoW
	{
		void OnWaitComissionExit();

		void OnWaitComissionEntry();

		bool CouldComission();
	}
}