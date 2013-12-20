using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Managers;
using Invest.Common;
using Invest.Common.Model.Common;
using Invest.Common.Model.User;
using MongoDB.Bson;

namespace BusinessLogic.Notification
{
    public class PortalNotificationHub
    {
        private static readonly Dictionary<string, PortalNotification> CahedUserNotificateInfo;
        private static readonly PortalMessageHandler MessageHandler;
        private static readonly TaskManager TaskManager;

        static PortalNotificationHub()
        {
            CahedUserNotificateInfo = new Dictionary<string, PortalNotification>();
            MessageHandler = new PortalMessageHandler();
            TaskManager = new TaskManager();
        }

        public PortalNotificationHub()
        {
        }

        public PortalNotification Notification(string userName)
        {
            return new PortalNotification();
        }
    }
}