using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using MongoDB.Bson;

namespace Investmogilev.UI.Portal.Controllers
{
	[Authorize]
	public class NotesController : Controller
	{
		public ActionResult All()
		{
			var model = new List<ProjectNotes>();
			foreach (Project project in RepositoryContext.Current.All<Project>())
			{
				model.AddRange(
					RepositoryContext.Current.All<ProjectNotes>(n => project != null && n.ProjectId == project.Id));
			}

			model.AddRange(RepositoryContext.Current.All<ProjectNotes>(n => n.CretorName == User.Identity.Name));

			foreach (string role in Roles.GetRolesForUser(User.Identity.Name))
			{
				model.AddRange(
					RepositoryContext.Current.All<ProjectNotes>(
						n => role != null && (n.RolesForView != null && n.RolesForView.Contains(role))));
			}

			return View(model.GroupBy(cust => cust.Id).Select(grp => grp.First()));
		}

		public ActionResult Index(string id)
		{
			var model = new List<ProjectNotes>();
			model.AddRange(RepositoryContext.Current.All<ProjectNotes>(n => n.ProjectId == id));
			return View(model);
		}

		public ActionResult Create(string proejctId)
		{
			var note = new ProjectNotes
			{
				ProjectId = proejctId,
				CretorName = User.Identity.Name,
				Id = ObjectId.GenerateNewId().ToString(),
				CreatedTime = DateTime.Now
			};
			RepositoryContext.Current.Add(note);
			return View(note);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(ProjectNotes model)
		{
			if (ModelState.IsValid)
			{
				model.NoteDocument =
					RepositoryContext.Current.GetOne<ProjectNotes>(p => p.Id == model.Id).NoteDocument;
				RepositoryContext.Current.Update(model);
				return RedirectToAction("All");
			}

			return View(model);
		}

		public ActionResult Edit(string id)
		{
			return View(RepositoryContext.Current.GetOne<ProjectNotes>(p => p.Id == id));
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(ProjectNotes model)
		{
			if (ModelState.IsValid)
			{
				model.NoteDocument =
					RepositoryContext.Current.GetOne<ProjectNotes>(p => p.Id == model.Id).NoteDocument;
				RepositoryContext.Current.Update(model);
				return RedirectToAction("All");
			}

			return View(model);
		}

		public ActionResult Delete(string id)
		{
			var note = RepositoryContext.Current.GetOne<ProjectNotes>(p => p.Id == id);
			RepositoryContext.Current.Delete(note);
			return RedirectToAction("All");
		}

		public ActionResult Details(string id)
		{
			return View(RepositoryContext.Current.GetOne<ProjectNotes>(p => p.Id == id));
		}

		public ActionResult Save(string id, IEnumerable<HttpPostedFileBase> attachments)
		{
			var note = RepositoryContext.Current.GetOne<ProjectNotes>(p => p.Id == id);
			var project = RepositoryContext.Current.GetOne<Project>(pr => pr.Id == note.ProjectId);
			var result = new List<AdditionalInfo>();
			if (note.NoteDocument != null)
			{
				result = note.NoteDocument.ToList();
			}
			else
			{
				note.NoteDocument = result;
			}


			foreach (HttpPostedFileBase file in attachments)
			{
				string fileName = Path.GetFileName(file.FileName);
				string physicalPath = Path.Combine(
					Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/", project.Name)),
					fileName);

				if (!Directory.Exists(
					Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/", project.Name))))
				{
					Directory.CreateDirectory(
						Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/", project.Name)));
				}

				file.SaveAs(physicalPath);
				var document = new DocumentAdditionalInfo();
				document.FilePath = physicalPath;
				document.InfoName = fileName;
				document.Id = ObjectId.GenerateNewId().ToString();
				result.Add(document);
			}

			note.NoteDocument = result;
			RepositoryContext.Current.Update(note);

			return Content("");
		}

		public FileResult Download(string noteId, string docId)
		{
			var note = RepositoryContext.Current.GetOne<ProjectNotes>(p => p.Id == noteId);
			var doc = note.NoteDocument.FirstOrDefault(t => t.Id == docId) as DocumentAdditionalInfo;
			if (doc != null) return File(doc.FilePath, "application/doc", doc.InfoName);
			return null;
		}
	}
}