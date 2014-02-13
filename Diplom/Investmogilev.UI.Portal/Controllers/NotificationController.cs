using System.Linq;
using System.Web.Mvc;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.User;

namespace Investmogilev.UI.Portal.Controllers
{
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
			var model = RepositoryContext.Current.GetOne<NotificationQueue>(q => q.Id == id);
			model.IsRead = true;
			RepositoryContext.Current.Update(model);
			return View(model);
		}
	}
}