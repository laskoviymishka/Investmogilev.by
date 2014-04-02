// // -----------------------------------------------------------------------
// // <copyright file="PortalNotificationHub.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	#region Using

	using System.Collections.Generic;
	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Model.User;

	#endregion

	public class PortalNotificationHub
	{
		private static readonly Dictionary<string, PortalNotification> CahedUserNotificateInfo;
		private static readonly PortalMessageHandler MessageHandler;

		static PortalNotificationHub()
		{
			CahedUserNotificateInfo = new Dictionary<string, PortalNotification>();
			MessageHandler = new PortalMessageHandler();
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