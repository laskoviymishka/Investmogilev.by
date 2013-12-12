using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Managers;
using Invest.Common.Model.Common;
using Invest.Common.Model.ProjectModels;
using MongoRepository;
using MongoDB.Bson;
using ProjectStates = Invest.Common.State.ProjectStates;

namespace InvestPortal.Controllers
{
    [Authorize(Roles = "Investor")]
    public class ReportController : Controller
    {
        public ActionResult Index()
        {
            var projects = RepositoryContext.Current.All<Project>(
                p => 
                    p.InvestorUser == User.Identity.Name &&
                    (p.WorkflowState.CurrenState == ProjectStates.PlanRealiztion
                    || p.WorkflowState.CurrenState == ProjectStates.DefectPlanRealization));

            var result = new List<Task>();
            foreach (Project project in projects)
            {
                if (project.Tasks != null)
                {
                    result.AddRange(CollectLeaf(project.Tasks.ToList()));
                }
            }
            return View(result);
        }

        public ActionResult UnComplete()
        {
            var projects = RepositoryContext.Current.All<Project>(
                p =>
                    p.InvestorUser == User.Identity.Name &&
                    (p.WorkflowState.CurrenState == ProjectStates.PlanRealiztion
                    || p.WorkflowState.CurrenState == ProjectStates.DefectPlanRealization));

            var result = new List<Task>();
            foreach (Project project in projects)
            {
                if (project.Tasks != null)
                {
                    result.AddRange(CollectLeaf(project.Tasks.ToList()));
                }
            }
            return View(result.Where(p => !p.IsComplete));
        }

        public IList<Task> CollectLeaf(IList<Task> item)
        {
            var result = new List<Task>();
            foreach (Task task in item)
            {
                if (task.SubTask == null || !task.SubTask.Any())
                {
                    result.Add(task);
                }
                else
                {
                    result.AddRange(CollectLeaf(task.SubTask.ToList()));
                }
            }
            return result;
        }

        public ActionResult CreateReport(string id)
        {
            var task = RepositoryContext.Current.GetOne<Task>(t => t._id == id);
            if (task.IsComplete)
            {
                return RedirectToAction("Index");
            }
            TaskReport report = new TaskReport();
            report.TaskId = task._id;
            report.ProjectId = task.ProjectId;
            report.ReporterName = User.Identity.Name;
            report.IsApproved = false;
            report.ReportTime = DateTime.Now;
            report._id = ObjectId.GenerateNewId().ToString();
            if (task.Reports == null)
            {
                task.Reports = new List<TaskReport>();
            }

            ((List<TaskReport>)task.Reports).Add(report);
            RepositoryContext.Current.Update(task);
            return View(report);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateReport(TaskReport model)
        {
            var task = RepositoryContext.Current.GetOne<Task>(t => t._id == model.TaskId);
            var initialReport = task.Reports.FirstOrDefault(t => t._id == model._id);
            if (initialReport != null) initialReport.Body = model.Body;
            task.IsComplete = true;
            task.IsVerifiedComplete = false;
            initialReport.IsVisible = true;
            RepositoryContext.Current.Update(task);
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            var task = RepositoryContext.Current.GetOne<Task>(p => p._id == id);
            return View(task);
        }

        public ActionResult Save(string taskId, string reportId, IEnumerable<HttpPostedFileBase> attachments)
        {
            var task = RepositoryContext.Current.GetOne<Task>(t => t._id == taskId);
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == task.ProjectId);
            var report = task.Reports.FirstOrDefault(r => r._id == reportId);
            var result = new List<AdditionalInfo>();
            foreach (var file in attachments)
            {
                string fileName = Path.GetFileName(file.FileName);
                string physicalPath = Path.Combine(
                    Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/Tasks/{1}/", project.Name,task.Title)),
                    fileName);

                if (!Directory.Exists(
                        Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/Tasks/{1}/", project.Name, task.Title))))
                {
                    Directory.CreateDirectory(Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/Tasks/{1}/", project.Name, task.Title)));
                }

                file.SaveAs(physicalPath);
                var document = new DocumentAdditionalInfo();
                document.FilePath = physicalPath;
                document.InfoName = fileName;
                document._id = ObjectId.GenerateNewId().ToString();
                result.Add(document);
            }
            report.IsVisible = false;
            report.Appendix = result;
            RepositoryContext.Current.Update(task);

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
