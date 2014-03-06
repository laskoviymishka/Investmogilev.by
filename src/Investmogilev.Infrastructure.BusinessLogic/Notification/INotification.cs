namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	public interface INotification
	{
		bool NotificateUser(string from, string to, string title, string body);
	}
}