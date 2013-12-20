using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Managers;
using Invest.Common;
using Invest.Common.Model.Common;
using Invest.Common.Model.Project;
using MongoDB.Bson;

namespace InvestPortal.Controllers
{
    [Authorize(Roles = "Investor")]
    public class ReportController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnComplete()
        {
            return View();
        }

        public ActionResult CreateReport(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateReport(Report model)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            return View();
        }

        public ActionResult Save(string taskId, string reportId, IEnumerable<HttpPostedFileBase> attachments)
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
