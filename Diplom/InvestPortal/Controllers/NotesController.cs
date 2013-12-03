using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Invest.Common.Model.Common;
using Invest.Common.Model.ProjectModels;
using MongoRepository;
using MongoDB.Bson;

namespace InvestPortal.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        public ActionResult All()
        {
            var model = new List<ProjectNotes>();
            foreach (var project in RepositoryContext.Current.All<Project>(p => p.AssignUser == User.Identity.Name))
            {
                model.AddRange(RepositoryContext.Current.All<ProjectNotes>(n => project != null && n.ProjectId == project._id));
            }

            model.AddRange(RepositoryContext.Current.All<ProjectNotes>(n => n.CretorName == User.Identity.Name));

            foreach (var role in Roles.GetRolesForUser(User.Identity.Name))
            {
                model.AddRange(
                    RepositoryContext.Current.All<ProjectNotes>(
                        n => role != null && (n.RolesForView != null && n.RolesForView.Contains(role))));
            }

            return View(model.GroupBy(cust => cust._id).Select(grp => grp.First()));
        }

        public ActionResult Index(string id)
        {
            var model = new List<ProjectNotes>();
            model.AddRange(RepositoryContext.Current.All<ProjectNotes>(n => n.ProjectId == id));
            return View(model);
        }

        public ActionResult Create(string id)
        {
            var note = new ProjectNotes
                {
                    ProjectId = id,
                    CretorName = User.Identity.Name,
                    _id = ObjectId.GenerateNewId().ToString(),
                    CreatedTime = DateTime.Now
                };
            RepositoryContext.Current.Add(note);
            return View(note);
        }

        [HttpPost]
        public ActionResult Create(ProjectNotes model)
        {
            if (ModelState.IsValid)
            {
                model.NoteDocument = RepositoryContext.Current.GetOne<ProjectNotes>(p => p._id == model._id).NoteDocument;
                RepositoryContext.Current.Update(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            return View(RepositoryContext.Current.GetOne<ProjectNotes>(p => p._id == id));
        }

        [HttpPost]
        public ActionResult Edit(ProjectNotes model)
        {
            if (ModelState.IsValid)
            {
                model.NoteDocument = RepositoryContext.Current.GetOne<ProjectNotes>(p => p._id == model._id).NoteDocument;
                RepositoryContext.Current.Update(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var note = RepositoryContext.Current.GetOne<ProjectNotes>(p => p._id == id);
            RepositoryContext.Current.Delete(note);
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            return View(RepositoryContext.Current.GetOne<ProjectNotes>(p => p._id == id));
        }

        public ActionResult Save(string id, IEnumerable<HttpPostedFileBase> attachments)
        {
            var note = RepositoryContext.Current.GetOne<ProjectNotes>(p => p._id == id);
            var project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == note.ProjectId);
            var result = new List<AdditionalInfo>();
            if (note.NoteDocument != null)
            {
                result = note.NoteDocument.ToList();
            }
            else
            {
                note.NoteDocument = result;
            }


            foreach (var file in attachments)
            {
                string fileName = Path.GetFileName(file.FileName);
                string physicalPath = Path.Combine(
                    Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/", project.Name)),
                    fileName);

                if (!Directory.Exists(
                        Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/", project.Name))))
                {
                    Directory.CreateDirectory(Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/", project.Name)));
                }

                file.SaveAs(physicalPath);
                var document = new DocumentAdditionalInfo();
                document.FilePath = physicalPath;
                document.InfoName = fileName;
                document._id = ObjectId.GenerateNewId().ToString();
                result.Add(document);
            }

            note.NoteDocument = result;
            RepositoryContext.Current.Update(note);

            return Content("");
        }

        public FileResult Download(string noteId, string docId)
        {
            var note = RepositoryContext.Current.GetOne<ProjectNotes>(p => p._id == noteId);
            var doc = note.NoteDocument.FirstOrDefault(t => t._id == docId) as DocumentAdditionalInfo;
            if (doc != null) return File(doc.FilePath, "application/doc", doc.InfoName);
            return null;
        }
    }
}
