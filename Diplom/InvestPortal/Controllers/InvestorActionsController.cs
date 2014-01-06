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
    public class InvestorActionsController : Controller
    {
        #region MainActions

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
                         && t.Type == TaskTypes.Document
                         && t.TaskReport == null
                         || (t.TaskReport != null
                             && t.TaskReport.Last().ReportResponse != null
                             && !t.TaskReport.Last().ReportResponse.IsApproved)));
            }
            return View(model);
        }

        #endregion

        #region CreateReport

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
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var reportManager = new ReportManager(model.TaskId, model._id, model.ProjectId);
            reportManager.CreateReport(model);
            return RedirectToAction("Index");
        }

        #endregion

        #region Details

        public ActionResult Details(string taskId, string projectId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            ProjectTask task = project.Tasks.Find(t => t._id == taskId);
            return View(task);
        }

        #endregion

        #region Appendix

        public ActionResult Save(string taskId, string reportId, string projectId,
            IEnumerable<HttpPostedFileBase> attachments)
        {
            foreach (var file in attachments)
            {
                var reportManager = new ReportManager(taskId, reportId, projectId);
                string fileName = Path.GetFileName(file.FileName);
                string physicalPath = reportManager.GetReportDocumentPhysicalPath();
                file.SaveAs(reportManager.GetReportDocumentPhysicalPath());
                reportManager.AddDocumentToReport(
                    new DocumentAdditionalInfo
                    {
                        FilePath = physicalPath + fileName,
                        InfoName = fileName,
                        _id = ObjectId.GenerateNewId().ToString()
                    });
            }

            return Content("");
        }

        public FileResult Download(string taskId, string reportId, string projectId, string docId)
        {
            var reportManager = new ReportManager(taskId, reportId, projectId);
            var doc = reportManager.GetReportAdditionalInfo(docId) as DocumentAdditionalInfo;
            return doc != null ? File(doc.FilePath, "application/doc", doc.InfoName) : null;
        }

        #endregion
    }
}