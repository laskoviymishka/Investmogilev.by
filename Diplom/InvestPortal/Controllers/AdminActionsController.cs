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
    public class AdminActionsController : Controller
    {
        private const string Filepath = "~/App_Data/ProjectInfo/{0}/Tasks/{1}/TaskReports/{2}/response/";

        public ActionResult Index()
        {
            IQueryable<Project> projects = RepositoryContext.Current.All<Project>(p => p.Tasks != null && p.Tasks.Any());
            var model = new List<ProjectTask>();
            foreach (Project project in projects)
            {
                foreach (
                    ProjectTask task in
                        project.Tasks.Where(t => t.TaskReport != null || t.Type == TaskTypes.InvolvedOrganiztion))
                {
                    task.ProjectId = project._id;
                    model.Add(task);
                }
            }
            return View(model);
        }

        public ActionResult UnVerified()
        {
            IQueryable<Project> projects = RepositoryContext.Current.All<Project>(p => p.Tasks != null && p.Tasks.Any());
            var model = new List<ProjectTask>();
            foreach (Project project in projects)
            {
                foreach (
                    ProjectTask task in
                        project.Tasks.Where(
                            t =>
                                t.TaskReport != null && t.TaskReport.Last().ReportResponse == null ||
                                t.Type == TaskTypes.InvolvedOrganiztion))
                {
                    task.ProjectId = project._id;
                    model.Add(task);
                }
            }
            return View(model);
        }

        public ActionResult Details(string taskId, string projectId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            ProjectTask task = project.Tasks.Find(t => t._id == taskId);
            return View(task);
        }

        public ActionResult ReviewReport(string taskId, string reportId, string projectId)
        {
            return View(
                new ReportResponse
                {
                    TaskId = taskId,
                    ProjectId = projectId,
                    ReportId = reportId,
                    ResponseTime = DateTime.Now,
                    _id = ObjectId.GenerateNewId().ToString()
                });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReviewReport(ReportResponse model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == model.ProjectId);
            ProjectTask task = project.Tasks.Find(t => t._id == model.TaskId);
            if (task.TaskReport == null || !task.TaskReport.Any())
            {
                task.TaskReport = new List<Report>();
            }

            Report report = task.TaskReport.Find(t => t._id == model.ReportId);

            if (report.ReportResponse == null)
            {
                report.ReportResponse = model;
            }
            else
            {
                report.ReportResponse.ResponseTime = model.ResponseTime;
                report.ReportResponse.Body = model.Body;
                report.ReportResponse.IsApproved = model.IsApproved;
            }

            if (report.ReportResponse.IsApproved)
            {
                task.IsComplete = true;
                task.CompleteTime = DateTime.Now;
            }

            RepositoryContext.Current.Update(project);

            ProjectStateManager.StateManagerFactory(
                project,
                User.Identity.Name,
                Roles.GetRolesForUser(User.Identity.Name))
                .DocumentUpdate();

            return RedirectToAction("Index");
        }

        public ActionResult Save(string taskId, string reportId, string reposponseId, string projectId,
            IEnumerable<HttpPostedFileBase> attachments)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            ProjectTask task = project.Tasks.Find(t => t._id == taskId);
            if (task.TaskReport == null || !task.TaskReport.Any())
            {
                task.TaskReport = new List<Report>();
            }

            Report report = task.TaskReport.Find(t => t._id == reportId);

            if (report.ReportResponse == null)
            {
                report.ReportResponse = new ReportResponse
                {
                    _id = reposponseId,
                    ProjectId = projectId,
                    TaskId = taskId,
                    ReportId = reportId,
                    Info = new List<AdditionalInfo>()
                };
            }


            foreach (HttpPostedFileBase file in attachments)
            {
                string fileName = Path.GetFileName(file.FileName);
                string physicalPath = Path.Combine(
                    Server.MapPath(string.Format(Filepath, project.Name, task.Title, DateTime.Now.ToShortDateString())),
                    fileName);

                if (!Directory.Exists(
                    Server.MapPath(
                        string.Format(
                            Filepath,
                            project.Name,
                            task.Title,
                            DateTime.Now.ToShortDateString()))))
                {
                    Directory.CreateDirectory(
                        Server.MapPath(string.Format(Filepath, project.Name, task.Title,
                            DateTime.Now.ToShortDateString())));
                }

                file.SaveAs(physicalPath);
                report.ReportResponse.Info.Add(
                    new DocumentAdditionalInfo
                    {
                        FilePath = physicalPath,
                        InfoName = fileName,
                        _id = ObjectId.GenerateNewId().ToString()
                    });
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

        public ActionResult InvolvedReport(string taskId, string projectId)
        {
            return View(
                new ReportResponse
                {
                    TaskId = taskId,
                    ProjectId = projectId,
                    ResponseTime = DateTime.Now,
                    _id = ObjectId.GenerateNewId().ToString()
                });
        }

        [HttpPost]
        public ActionResult InvolvedReport(ReportResponse model)
        {
            if (!ModelState.IsValid) return View(model);

            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == model.ProjectId);
            ProjectTask task = project.Tasks.Find(t => t._id == model.TaskId);
            if (task.TaskReport == null)
            {
                task.TaskReport = new List<Report>();
            }

            var report = new Report
            {
                _id = ObjectId.GenerateNewId().ToString(),
                ReportTime = DateTime.Now,
                Body = model.Body,
                ReportResponse = model
            };

            task.TaskReport.Add(report);
            task.IsComplete = model.IsApproved;
            RepositoryContext.Current.Update(project);

            if (project.Tasks.Any(t => t.Type == TaskTypes.InvolvedOrganiztion && t.TaskReport == null))
            {
                ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
                    Roles.GetRolesForUser(User.Identity.Name)).InvolvedOrganizationUpdate();
            }
            else
            {
                ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
                    Roles.GetRolesForUser(User.Identity.Name)).ToComission();
            }

            return RedirectToAction("Project", "BaseProject", new {id = project._id});
        }

        public ActionResult CreateComission()
        {
            throw new NotImplementedException();
        }
    }
}