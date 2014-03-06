using Investmogilev.Infrastructure.Common.Model.Project;

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	public interface IUserNotification
	{
		void NotificateOpen(Project currentProject);
		void InvestorResponsed(Project currentProject);
	}
}