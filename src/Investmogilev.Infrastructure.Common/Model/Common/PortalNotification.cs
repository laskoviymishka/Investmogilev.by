// // -----------------------------------------------------------------------
// // <copyright file="PortalNotification.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	public class PortalNotification
	{
		public int UnreadMessages { get; set; }
		public int ActiveTask { get; set; }
		public int PendingTask { get; set; }
		public int CompleteTask { get; set; }
		public int PendingCompleteTask { get; set; }
		public int UnReadNotification { get; set; }
	}
}