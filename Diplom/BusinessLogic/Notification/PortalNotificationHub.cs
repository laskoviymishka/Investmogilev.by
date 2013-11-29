using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Managers;
using Invest.Common.Model.Common;

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
            if (!CahedUserNotificateInfo.ContainsKey(userName))
            {
                var tasks = TaskManager.GetAllTask(userName);
                var portalNotification = new PortalNotification
                {
                    UnreadMessages = MessageHandler.Unread(userName).Count(),
                    ActiveTask = tasks.Count(t => !t.IsComplete),
                    PendingTask = tasks.Count(t => !t.IsComplete && t.Milestone < DateTime.Now),
                    CompleteTask = tasks.Count(t => t.IsComplete),
                    PendingCompleteTask = tasks.Count(t => t.IsComplete && t.CompletedOn < DateTime.Now)
                };

                CahedUserNotificateInfo.Add(userName, portalNotification);
            }

            return CahedUserNotificateInfo[userName];
        }

        public void UpdateNotficationForUser(string userName)
        {
            var tasks = TaskManager.GetAllTask(userName);
            var portalNotification = new PortalNotification
            {
                UnreadMessages = MessageHandler.Unread(userName).Count(),
                ActiveTask = tasks.Count(t => !t.IsComplete),
                PendingTask = tasks.Count(t => !t.IsComplete && t.Milestone < DateTime.Now),
                CompleteTask = tasks.Count(t => t.IsComplete),
                PendingCompleteTask = tasks.Count(t => t.IsComplete && t.CompletedOn < DateTime.Now)
            };

            if (!CahedUserNotificateInfo.ContainsKey(userName))
            {
                CahedUserNotificateInfo.Add(userName, portalNotification);
            }
            else
            {
                CahedUserNotificateInfo[userName] = portalNotification;
            }

        }
    }
}