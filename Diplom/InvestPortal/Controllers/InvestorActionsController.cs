using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invest.Common;
using Invest.Common.Model.Common;
using Invest.Common.Model.Project;
using MongoDB.Bson;

namespace InvestPortal.Controllers
{
    [Authorize(Roles = "Investor")]
    public class InvestorActionsController : Controller
    {
        private const string Filepath = "~/App_Data/ProjectInfo/{0}/Tasks/{1}/TaskReports/{2}";

        public ActionResult Index()
        {
            IQueryable<Project> projects =
                RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name);
            var model = new List<ProjectTask>();
            foreach (Project project in projects)
            {
                foreach (ProjectTask task in project.Tasks)
                {
                    task.ProjectId = project._id;
                    model.Add(task);
                }
            }
            return View(model);
        }

        public ActionResult UnComplete()
        {
            IQueryable<Project> projects =
                RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name);
            var model = new List<ProjectTask>();
            foreach (Project project in projects)
            {
                model.AddRange(project.Tasks.Where(
                    t => !t.IsComplete
                         && t.TaskReport == null
                         || (t.TaskReport != null
                             && t.TaskReport.Last().ReportResponse != null
                             && !t.TaskReport.Last().ReportResponse.IsApproved)));
            }
            return View(model);
        }

        public ActionResult CreateReport(string taskId, string projectId)
        {
            return View(
                new Report
                {
                    TaskId = taskId,
                    ProjectId = projectId,
                    ReportTime = DateTime.Now,
                    _id = ObjectId.GenerateNewId().ToString()
                });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateReport(Report model)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == model.ProjectId);
            ProjectTask task = project.Tasks.Find(t => t._id == model.TaskId);
            if (task.TaskReport == null || !task.TaskReport.Any())
            {
                task.TaskReport = new List<Report>();
            }
            Report report;
            if (task.TaskReport.Find(t => t._id == model._id) != null)
            {
                report = task.TaskReport.Find(t => t._id == model._id);
                report.Body = model.Body;
                report.ReportTime = model.ReportTime;
            }
            else
            {
                report = model;
                task.TaskReport.Add(report);
            }


            RepositoryContext.Current.Update(project);

            return RedirectToAction("Index");
        }

        public ActionResult Details(string taskId, string projectId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            ProjectTask task = project.Tasks.Find(t => t._id == taskId);
            return View(task);
        }

        public ActionResult Save(string taskId, string reportId, string projectId,
            IEnumerable<HttpPostedFileBase> attachments)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            ProjectTask task = project.Tasks.Find(t => t._id == taskId);
            if (task.TaskReport == null || !task.TaskReport.Any())
            {
                task.TaskReport = new List<Report>();
            }

            Report report;

            if (task.TaskReport.Find(t => t._id == reportId) != null)
            {
                report = task.TaskReport.Find(t => t._id == reportId);
            }
            else
            {
                report = new Report {ProjectId = projectId, _id = reportId, TaskId = taskId};
                task.TaskReport.Add(report);
            }

            if (report.Info == null || !report.Info.Any())
            {
                report.Info = new List<AdditionalInfo>();
            }

            foreach (HttpPostedFileBase file in attachments)
            {
                string fileName = Path.GetFileName(file.FileName);
                string physicalPath = Path.Combine(
                    Server.MapPath(string.Format(Filepath, project.Name, task.Title, DateTime.Now.ToShortDateString())),
                    fileName);

                if (!Directory.Exists(
                    Server.MapPath(string.Format(Filepath, project.Name, task.Title, DateTime.Now.ToShortDateString()))))
                {
                    Directory.CreateDirectory(
                        Server.MapPath(string.Format(Filepath, project.Name, task.Title,
                            DateTime.Now.ToShortDateString())));
                }

                file.SaveAs(physicalPath);
                var document = new DocumentAdditionalInfo();
                document.FilePath = physicalPath;
                document.InfoName = fileName;
                document._id = ObjectId.GenerateNewId().ToString();
                report.Info.Add(document);
            }

            RepositoryContext.Current.Update(project);

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