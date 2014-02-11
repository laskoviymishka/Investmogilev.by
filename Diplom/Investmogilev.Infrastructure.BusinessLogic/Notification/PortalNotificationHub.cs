﻿using System.Collections.Generic;
using Investmogilev.Infrastructure.BusinessLogic.Managers;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.User;

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
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

		public void PushNotificate(NotificationQueue notification)
		{
			RepositoryContext.Current.Add(notification);
		}
	}
}