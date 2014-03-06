namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IInvolvedorganizationsUoW
	{
		void OnInvolvedOrganizationsExit();
		void OnInvolvedOrganizationsEntry();
		bool CouldInvolvedOrganizationUpdate();
		bool CouldInvolvedOrganizationUpdateAndLeave();
	}
}