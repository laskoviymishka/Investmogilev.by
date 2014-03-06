namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IIspolcomFixesUoW
	{
		void OnWaitIspolcomFixesExit();

		void OnWaitIspolcomFixesEntry();

		bool CouldIspolcomFixUpdate();

		bool CouldIspolcomFixUpdateAndLeave();
	}
}