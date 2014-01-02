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
            var projects = RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name);
            List<ProjectTask> model = new List<ProjectTask>();
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
            var projects = RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name);
            List<ProjectTask> model = new List<ProjectTask>();
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
            Report model = new Report();
            model.TaskId = taskId;
            model.ProjectId = projectId;
            model.ReportTime = DateTime.Now;
            model._id = ObjectId.GenerateNewId().ToString();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateReport(Report model)
        {
            Project project = RepositoryContext.Current.GetOne<Project>(p => p._id == model.ProjectId);
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

        public ActionResult Details(string id)
        {
            return View();
        }

        public ActionResult Save(string taskId, string reportId, string projectId, IEnumerable<HttpPostedFileBase> attachments)
        {
            string filepath = "~/App_Data/ProjectInfo/{0}/Tasks/{1}/TaskReports/{2}";
            Project project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
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
                report = new Report();
                report.ProjectId = projectId;
                report._id = reportId;
                report.TaskId = taskId;
                task.TaskReport.Add(report);
            }

            if (report.Info == null || !report.Info.Any())
            {
                report.Info = new List<AdditionalInfo>();
            }

            string physicalPath = "";
            string fileName = "";
            foreach (var file in attachments)
            {
                fileName = Path.GetFileName(file.FileName);
                physicalPath = Path.Combine(
                    Server.MapPath(string.Format(filepath, project.Name, task.Title, DateTime.Now.ToShortDateString())),
                    fileName);

                if (!Directory.Exists(
                        Server.MapPath(string.Format(filepath, project.Name, task.Title, DateTime.Now.ToShortDateString()))))
                {
                    Directory.CreateDirectory(Server.MapPath(string.Format(filepath, project.Name, task.Title, DateTime.Now.ToShortDateString())));
                }

                file.SaveAs(physicalPath);
                DocumentAdditionalInfo document = new DocumentAdditionalInfo();
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
