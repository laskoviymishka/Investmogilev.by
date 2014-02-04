using System.Web.Mvc;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using MongoDB.Bson;

namespace Investmogilev.UI.Portal.Controllers
{
	public class TaskTemplateController : Controller
	{
		public ActionResult Index()
		{
			return View(RepositoryContext.Current.All<TaskTemplate>());
		}

		public ActionResult Details(string id)
		{
			return View(RepositoryContext.Current.GetOne<TaskTemplate>(t => t._id == id));
		}

		public ActionResult Create()
		{
			return View(new TaskTemplate {_id = ObjectId.GenerateNewId().ToString()});
		}

		[HttpPost]
		public ActionResult Create(TaskTemplate template)
		{
			if (ModelState.IsValid)
			{
				RepositoryContext.Current.Add(template);
				return RedirectToAction("Index");
			}

			return View(template);
		}

		public ActionResult Edit(string id)
		{
			return View(RepositoryContext.Current.GetOne<TaskTemplate>(t => t._id == id));
		}

		[HttpPost]
		public ActionResult Edit(TaskTemplate template)
		{
			if (ModelState.IsValid)
			{
				RepositoryContext.Current.Update(template);
			}

			return View(template);
		}

		public ActionResult Delete(string id)
		{
			RepositoryContext.Current.Delete<TaskTemplate>(t => t._id == id);
			return RedirectToAction("Index");
		}
	}
}