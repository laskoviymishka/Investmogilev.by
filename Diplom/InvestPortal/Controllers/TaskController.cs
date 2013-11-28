using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Managers;
using Invest.Common.Model.Common;
using Invest.Common.Model.ProjectModels;
using InvestPortal.Models;
using MongoDB.Bson;
using MongoRepository;

namespace InvestPortal.Controllers
{
    public class TaskController : Controller
    {
        #region Private Fields

        private readonly TaskManager _taskManager;

        #endregion

        #region Constructor

        public TaskController()
        {
            _taskManager = new TaskManager();
        }

        #endregion

        #region Task Crud

        [Authorize]
        public ActionResult MyTask()
        {
            if (User.IsInRole("Admin") || User.IsInRole("User"))
            {
                return View(_taskManager.UserProjects(User.Identity.Name));
            }

            if (User.IsInRole("Investor"))
            {
                return View(_taskManager.InvestorProjects(User.Identity.Name));
            }

            return HttpNotFound("Роль пользователя не определена свяжитесь с администрацией");
        }

        [Authorize]
        public ActionResult ActiveTask()
        {
            if (User.IsInRole("Admin") || User.IsInRole("User"))
            {
                return View(_taskManager.UserProjects(User.Identity.Name));
            }

            if (User.IsInRole("Investor"))
            {
                return View(_taskManager.InvestorProjects(User.Identity.Name));
            }

            return HttpNotFound("Роль пользователя не определена свяжитесь с администрацией");
        }


        [Authorize]
        public ActionResult CompleteTask()
        {
            if (User.IsInRole("Admin") || User.IsInRole("User"))
            {
                return View(_taskManager.UserProjects(User.Identity.Name));
            }

            if (User.IsInRole("Investor"))
            {
                return View(_taskManager.InvestorProjects(User.Identity.Name));
            }

            return HttpNotFound("Роль пользователя не определена свяжитесь с администрацией");
        }

        [Authorize(Roles = "User")]
        public ActionResult PlanForProject(string id)
        {
            return View(_taskManager.GetProject(id));
        }

        [Authorize(Roles = "User")]
        public ActionResult TaskDetails(string taskId, string projectId)
        {
            ViewBag.ProjectId = projectId;
            var model = new EditTaskViewModel(_taskManager.GetTask(taskId, projectId));
            model.ProjectId = projectId;
            return View(model);
        }

        [Authorize(Roles = "User")]
        public ActionResult TaskEdit(string taskId, string projectId)
        {
            ViewBag.ProjectId = projectId;
            var model = new EditTaskViewModel(_taskManager.GetTask(taskId, projectId));
            model.ProjectId = projectId;
            return View(model);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult TaskEdit(EditTaskViewModel task)
        {
            _taskManager.UpdateTask(task, task.ProjectId);
            return RedirectToAction("TaskDetails", new { taskId = task.TaskId, projectId = task.ProjectId });
        }

        #endregion

        #region Create task

        [Authorize(Roles = "User")]
        public ActionResult CreateSubTask(string taskId, string projectId)
        {
            var viewModel = new SubTaskViewModel();
            viewModel.ParentId = taskId;
            viewModel.ProjectId = projectId;
            return PartialView(viewModel);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult CreateSubTask(SubTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = new Task
                    {
                        Title = model.Name,
                        Description = model.Description,
                        Milestone = model.Milestone,
                        TaskId = ObjectId.GenerateNewId().ToString()
                    };

                if (_taskManager.CreateTask(task, model.ProjectId, model.ParentId))
                {
                    return RedirectToAction("PlanForProject", "Task", new { @id = model.ProjectId });

                }

                return HttpNotFound("Возникла ошибка, свяжитесь с администратором");

            }

            return View(model);
        }

        #endregion

        #region Additional info for Tasks

        public ActionResult AdditionalInfo(string taskId, string projectId)
        {
            ViewBag.ProjectId = projectId;
            return View(_taskManager.GetTask(taskId, projectId));
        }

        public ActionResult AddDocumentToTask(string taskId, string projectId)
        {
            return View(new DocumentForTaskViewModel() { ProjectId = projectId, TaskId = taskId, _id = ObjectId.GenerateNewId().ToString() });
        }

        public ActionResult EditDocumentInfo(string taskId, string projectId, string infoId)
        {
            return View(new DocumentForTaskViewModel(_taskManager.DocumentForTaks(projectId, taskId, infoId)));
        }

        [HttpPost]
        public ActionResult EditDocumentInfo(DocumentForTaskViewModel model)
        {
            _taskManager.UpdateDocument(model.ProjectId,model.TaskId,model._id,model);
            return RedirectToAction("AdditionalInfo", "Task", new { @taskId = model.TaskId, @projectId = model.ProjectId });
        }

        public ActionResult Save(string taskId, string projectId, string infoId, IEnumerable<HttpPostedFileBase> attachments)
        {
            string physicalPath = "";
            string fileName = "";
            foreach (var file in attachments)
            {
                fileName = Path.GetFileName(file.FileName);
                physicalPath = Path.Combine(
                    Server.MapPath(string.Format("~/App_Data/", projectId, taskId)),
                    fileName);
                file.SaveAs(physicalPath);
            }
            _taskManager.DocumentUplaod(projectId, taskId, infoId, fileName, physicalPath);
            return Content("");
        }
        public ActionResult Remove(string taskId, string projectId, string infoId)
        {
            _taskManager.DocumentRemove(projectId, taskId, infoId);
            return RedirectToAction("AdditionalInfo", "Task", new { @taskId = taskId, @projectId = projectId });
        }


        [Authorize(Roles = "User")]
        public FileResult Download(string taskId, string projectId, string infoId)
        {
            var doc = _taskManager.DocumentForTaks(projectId, taskId, infoId);
            return File(doc.FilePath, "application/doc", doc.InfoName);
        }

        #endregion
    }
}
