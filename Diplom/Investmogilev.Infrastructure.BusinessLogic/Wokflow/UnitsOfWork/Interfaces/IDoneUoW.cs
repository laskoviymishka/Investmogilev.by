namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IDoneUoW
	{
		void OnDoneExit();

		void OnDoneEntry();
	}
}