// // -----------------------------------------------------------------------
// // <copyright file="INotification.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	public interface INotification
	{
		bool NotificateUser(string from, string to, string title, string body);
	}
}