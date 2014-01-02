using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common;
using Invest.Common.Model.Common;
using Invest.Common.Model.Project;
using MongoDB.Bson;

namespace InvestPortal.Controllers
{
    [Authorize]
    public class ReportReviewController : Controller
    {
        public ActionResult Index()
        {
            var projects = RepositoryContext.Current.All<Project>(p => p.Tasks != null && p.Tasks.Any());
            List<ProjectTask> model = new List<ProjectTask>();
            foreach (Project project in projects)
            {
                foreach (ProjectTask task in project.Tasks.Where(t => t.TaskReport != null && t.TaskReport.Last().ReportResponse == null))
                {
                    task.ProjectId = project._id;
                    model.Add(task);
                }
            }
            return View(model);
        }

        public ActionResult UnVerified()
        {
            return View();
        }

        public ActionResult Details(string taskId)
        {
            return View();
        }

        public ActionResult ReviewReport(string taskId, string reportId)
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReviewReport(ReportResponse model)
        {
            return View();
        }


        public ActionResult Save(string taskId, string reportId, string reposponseId, IEnumerable<HttpPostedFileBase> attachments)
        {
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
