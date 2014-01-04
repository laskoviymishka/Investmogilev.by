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
        #region MainAction

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

        #endregion

        #region Review Report

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

            var reportManager = new ReportManager(model.TaskId, model.ReportId, model.ProjectId);
            reportManager.CreateReportResponse(model);

            ProjectStateManager.StateManagerFactory(
                reportManager.CurrentProject,
                User.Identity.Name,
                Roles.GetRolesForUser(User.Identity.Name))
                .DocumentUpdate();

            return RedirectToAction("Index");
        }

        #endregion

        #region Involved Report

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

        #endregion

        #region Comission

        public ActionResult CreateComission()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Appendix

        public ActionResult Save(string taskId, string reportId, string responsedId, string projectId,
            IEnumerable<HttpPostedFileBase> attachments)
        {
            foreach (HttpPostedFileBase file in attachments)
            {
                var reportManager = new ReportManager(taskId, reportId, projectId);
                string fileName = Path.GetFileName(file.FileName);
                string physicalPath = reportManager.GetReportResponseDocumentPhysicalPath();
                file.SaveAs(physicalPath);
                reportManager.AddDocumentToReportResponse(responsedId, new DocumentAdditionalInfo
                {
                    FilePath = physicalPath,
                    InfoName = fileName,
                    _id = ObjectId.GenerateNewId().ToString()
                });
            }

            return Content("");
        }

        public FileResult Download(string taskId, string reportId, string responsedId, string projectId)
        {
            var reportManager = new ReportManager(taskId, reportId, projectId);
            var doc = reportManager.GetReportResponseAdditionalInfo(responsedId) as DocumentAdditionalInfo;
            return doc != null ? File(doc.FilePath, "application/doc", doc.InfoName) : null;
        }

        #endregion
    }
}