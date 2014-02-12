namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IComissionFixesUoW
	{
		void OnWaitComissionFixesExit();

		void OnWaitComissionFixesEntry();

		bool CouldUpdateComissionFix();

		bool CouldUpdateComissionFixAndLeave();
	}
}