namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IOnComissionUoW
	{
		void OnOnComissionExit();

		void OnOnComissionEntry();

		bool CouldComissionFix();

		bool CouldToIspolcom();
	}
}