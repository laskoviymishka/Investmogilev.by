// // -----------------------------------------------------------------------
// // <copyright file="NotificationController.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Controllers
{
	#region Using

	using System.Linq;
	using System.Web.Mvc;
	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.User;

	#endregion

	public class NotificationController : Controller
	{
		public ActionResult Index()
		{
			return
				View(
					RepositoryContext.Current.All<NotificationQueue>(q => q.UserName == User.Identity.Name && !q.IsRead)
						.OrderBy(t => t.NotificationTime));
		}

		public ActionResult All()
		{
			return View(RepositoryContext.Current.All<NotificationQueue>(q => q.UserName == User.Identity.Name));
		}

		public ActionResult Read(string id)
		{
			var model = RepositoryContext.Current.GetOne<NotificationQueue>(q => q._id == id);
			model.IsRead = true;
			RepositoryContext.Current.Update(model);
			return View(model);
		}
	}
}