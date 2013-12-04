using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Managers;
using Invest.Common.Model.Common;
using Invest.Common.Model.User;
using MongoDB.Bson;
using MongoRepository;

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
            var tasks = TaskManager.GetAllTask(userName).ToArray();
            var portalNotification = new PortalNotification
            {
                UnreadMessages = MessageHandler.Unread(userName).Count(),
                ActiveTask = tasks.Count(t => !t.IsComplete),
                PendingTask = tasks.Count(t => !t.IsComplete && t.Milestone < DateTime.Now),
                CompleteTask = tasks.Count(t => t.IsComplete),
                PendingCompleteTask = tasks.Count(t => t.IsComplete && t.CompletedOn < t.Milestone),
                UnReadNotification = RepositoryContext.Current.All<NotificationQueue>(n => n.UserName == userName && !n.IsRead).Count()
            };
            return portalNotification;
        }
    }
}