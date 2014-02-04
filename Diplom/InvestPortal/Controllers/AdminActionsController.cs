using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Investmogilev.Infrastructure.BusinessLogic.Managers;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.State;
using MongoDB.Bson;

namespace Investmogilev.UI.Portal.Controllers
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

            if (task.Step == ProjectWorkflow.State.WaitInvolved)
            {
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
            }

            return RedirectToAction("Project", "BaseProject", new { id = project._id });
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

        #region Comission

        public ActionResult Comission()
        {
            return View(new Comission
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                });
        }

        [HttpPost]
        public ActionResult Comission(Comission comission)
        {
            if (!ModelState.IsValid)
            {
                return View(comission);
            }

            if (comission.CommissionTime < DateTime.Now)
            {
                ModelState.AddModelError("CommissionTime", "Время комиссии должно быть больше чем текущее время");
                return View(comission);
            }

            RepositoryContext.Current.Add(comission);

            foreach (var project in comission.Projects)
            {
                if (comission.Type == ComissionType.Comission)
                {
                    ProjectStateManager.StateManagerFactory(project, User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name)).Comission();
                }
                else
                {
                    ProjectStateManager.StateManagerFactory(project, User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name)).Ispolcom();
                }
            }

            return RedirectToAction("All", "BaseProject");
        }

        #endregion
    }
}