using Invest.Common.Model;

namespace Invest.Common.Notification
{
    public interface INotification
    {
        bool NotificateUser(string from, string to, string title, string body);
    }
}