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
using Invest.Common.Model.ProjectModels;
using MongoDB.Bson;

namespace InvestPortal.Controllers
{
    [Authorize]
    public class ReportReviewController : Controller
    {
        #region Private fields

        private readonly ProjectStateManager _stateManager;

        #endregion

        #region Constructor

        public ReportReviewController()
        {
            _stateManager = new ProjectStateManager();
        }

        #endregion

        public ActionResult Index()
        {
            var tasks = RepositoryContext.Current.All<Task>(t => t.AssignUser == User.Identity.Name && t.Reports != null && t.Reports.Any());
            var model = new List<TaskReport>();
            foreach (Task task in tasks)
            {
                model.AddRange(task.Reports);
            }
            return View(model);
        }

        public ActionResult UnVerified()
        {
            var tasks = RepositoryContext.Current.All<Task>(t => t.AssignUser == User.Identity.Name && t.Reports != null && t.Reports.Any());
            var model = new List<TaskReport>();
            foreach (Task task in tasks.Where(t => !t.IsVerifiedComplete))
            {
                model.AddRange(task.Reports);
            }
            return View(model);
        }

        public ActionResult Details(string taskId)
        {
            var task = RepositoryContext.Current.GetOne<Task>(p => p._id == taskId);
            return View(task);
        }

        public ActionResult ReviewReport(string taskId, string reportId)
        {
            var task = RepositoryContext.Current.GetOne<Task>(t => t._id == taskId);
            ReportResponse response = new ReportResponse();
            response.TaskId = taskId;
            response.ReportId = reportId;
            response._id = ObjectId.GenerateNewId().ToString();
            response.ResponseUser = User.Identity.Name;
            response.ResponseTime = DateTime.Now;
            var report = task.Reports.FirstOrDefault(t => t._id == reportId);
            if (report == null)
            {
                return HttpNotFound("отчет не найден");
            }

            report.Response = response;
            report.Response.IsReject = false;
            RepositoryContext.Current.Update(task);
            return View(response);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReviewReport(ReportResponse model)
        {
            if (ModelState.IsValid)
            {
                var task = RepositoryContext.Current.GetOne<Task>(t => t._id == model.TaskId);
                var report = task.Reports.FirstOrDefault(r => r._id == model.ReportId);
                if (report != null)
                {
                    report.IsApproved = model.IsApproved;
                    report.Response.IsApproved = model.IsApproved;
                    report.Response.ResponseBody = model.ResponseBody;
                    report.Response.IsReject = !model.IsApproved;
                }
                else
                {
                    return HttpNotFound("Отчет не найден");
                }

                if (model.IsApproved)
                {
                    task.IsComplete = true;
                    task.IsVerifiedComplete = true;
                    task.CompletedOn = model.ResponseTime;
                }
                else
                {
                    task.IsComplete = false;
                    task.IsVerifiedComplete = false;
                }

                RepositoryContext.Current.Update(task);

                var project = RepositoryContext.Current.GetOne<Project>(p => p._id == task.ProjectId);
                foreach (Task otherTask in project.Tasks)
                {
                    if (!(otherTask.IsComplete && otherTask.IsVerifiedComplete))
                    {
                        return RedirectToAction("Index");
                    }
                }
                _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
                _stateManager.CompletePlan(User.Identity.Name, project._id);
                return RedirectToAction("WorkFlowForProject", "BaseProject", new { id = project._id });
            }

            return View(model);
        }


        public ActionResult Save(string taskId, string reportId, string reposponseId, IEnumerable<HttpPostedFileBase> attachments)
        {
            var task = RepositoryContext.Current.GetOne<Task>(t => t._id == taskId);
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == task.ProjectId);
            var report = task.Reports.FirstOrDefault(r => r._id == reportId);
            var result = new List<AdditionalInfo>();
            foreach (var file in attachments)
            {
                string fileName = Path.GetFileName(file.FileName);
                string physicalPath = Path.Combine(
                    Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/Tasks/{1}/responsesBy{2}", project.Name, task.Title, report.Response.ResponseUser)),
                    fileName);

                if (!Directory.Exists(
                        Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/Tasks/{1}/responsesBy{2}", project.Name, task.Title, report.Response.ResponseUser))))
                {
                    Directory.CreateDirectory(Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/Tasks/{1}/responsesBy{2}", project.Name, task.Title, report.Response.ResponseUser)));
                }

                file.SaveAs(physicalPath);
                var document = new DocumentAdditionalInfo();
                document.FilePath = physicalPath;
                document.InfoName = fileName;
                document._id = ObjectId.GenerateNewId().ToString();
                result.Add(document);
            }

            report.Response.Appendix = result;
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
