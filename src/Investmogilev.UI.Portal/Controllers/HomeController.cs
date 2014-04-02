// // -----------------------------------------------------------------------
// // <copyright file="HomeController.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Controllers
{
	#region Using

	using System.Web.Mvc;

	#endregion

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