using System.Web.Mvc;

namespace Investmogilev.UI.Portal.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return Redirect("/map");
		}

		public ActionResult About()
		{
			return View();
		}
	}
}