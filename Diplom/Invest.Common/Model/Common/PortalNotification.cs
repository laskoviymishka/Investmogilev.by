namespace Invest.Common.Model.Common
{
    public class PortalNotification
    {
        public int UnreadMessages { get; set; }
        public int ActiveTask { get; set; }
        public int PendingTask { get; set; }
        public int CompleteTask { get; set; }
        public int PendingCompleteTask { get; set; }
    }
}