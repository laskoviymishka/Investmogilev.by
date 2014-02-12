namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IPlanCreatingUoW
	{
		void OnPlanCreatingExit();

		void OnPlanCreatingEntry();

		bool CouldUpdatePlan();

		bool CouldApprovePlan();
	}
}