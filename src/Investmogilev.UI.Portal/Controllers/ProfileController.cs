// // -----------------------------------------------------------------------
// // <copyright file="ProfileController.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Controllers
{
	#region Using

	using System.Web.Mvc;
	using Investmogilev.Infrastructure.BusinessLogic.Notification;
	using Investmogilev.Infrastructure.Common.Model.Common;

	#endregion

	public class ProfileController : Controller
	{
		private readonly PortalNotificationHub _notificationHub;

		public ProfileController()
		{
			_notificationHub = new PortalNotificationHub();
		}

		public ActionResult ProfilePartial()
		{
			if (User.Identity.IsAuthenticated)
			{
				return PartialView(new PortalNotification());
			}

			return PartialView();
		}

		public ActionResult MenuPartial()
		{
			return PartialView();
		}
	}
}