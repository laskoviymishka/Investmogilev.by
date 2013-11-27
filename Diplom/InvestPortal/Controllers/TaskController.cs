using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invest.Common.Model.ProjectModels;
using MongoRepository;
using InvestPortal.Models;
using MongoDB.Bson;
using Telerik.Web.Mvc.UI.Fluent;

namespace InvestPortal.Controllers
{
    public class TaskController : Controller
    {
        [Authorize]
        public ActionResult MyTask()
        {
            List<Project> myProjects;
            if (User.IsInRole("Admin") || User.IsInRole("Emploee"))
            {
                myProjects = RepositoryContext.Current.All<Project>(p => p.AssignUser == User.Identity.Name).ToList();
            }
            else
            {
                if (User.IsInRole("Investor"))
                {
                    myProjects =
                        RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name).ToList();
                }
                else
                {
                    return HttpNotFound("Роль пользователя не определена свяжитесь с администрацией");
                }
            }

            return View(myProjects);
        }

        [Authorize]
        public ActionResult ActiveTask()
        {
            List<Project> myProjects;
            if (User.IsInRole("Admin") || User.IsInRole("Emploee"))
            {
                myProjects = RepositoryContext.Current.All<Project>(p => p.AssignUser == User.Identity.Name).ToList();
            }
            else
            {
                if (User.IsInRole("Investor"))
                {
                    myProjects =
                        RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name).ToList();
                }
                else
                {
                    return HttpNotFound("Роль пользователя не определена свяжитесь с администрацией");
                }
            }

            return View(myProjects);
        }


        [Authorize]
        public ActionResult CompleteTask()
        {
            List<Project> myProjects;
            if (User.IsInRole("Admin") || User.IsInRole("Emploee"))
            {
                myProjects = RepositoryContext.Current.All<Project>(p => p.AssignUser == User.Identity.Name).ToList();
            }
            else
            {
                if (User.IsInRole("Investor"))
                {
                    myProjects =
                        RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name).ToList();
                }
                else
                {
                    return HttpNotFound("Роль пользователя не определена свяжитесь с администрацией");
                }
            }

            return View(myProjects);
        }

        [Authorize(Roles = "User")]
        public ActionResult PlanForProject(string id)
        {
            Project project = RepositoryContext.Current.GetOne<Project>(p => p._id == id);
            return View(project);
        }

        [Authorize(Roles = "User")]
        public ActionResult TaskDetails(string taskId, string projectId)
        {
            ViewBag.ProjectId = projectId;
            Project project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            Task task = HandleTreeItems(project.Tasks, taskId);
            var model = new EditTaskViewModel(task);
            model.ProjectId = projectId;
            return View(model);
        }

        [Authorize(Roles = "User")]
        public ActionResult TaskEdit(string taskId, string projectId)
        {
            ViewBag.ProjectId = projectId;
            Project project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            Task task = HandleTreeItems(project.Tasks, taskId);
            var model = new EditTaskViewModel(task);
            model.ProjectId = projectId;
            return View(model);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult TaskEdit(EditTaskViewModel task)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == task.ProjectId);
            var initialTask = HandleTreeItems(project.Tasks, task.TaskId);
            initialTask.CompletedOn = task.CompletedOn;
            initialTask.Description = task.Description;
            initialTask.Title = task.Title;
            initialTask.IsComplete = task.IsComplete;
            initialTask.IsVerifiedComplete = task.IsVerifiedComplete;
            initialTask.Milestone = task.Milestone;
            RepositoryContext.Current.Update(project);
            return View(task);
        }

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
                Project project = RepositoryContext.Current.GetOne<Project>(p => p._id == model.ProjectId);
                if (model.ProjectId == model.ParentId)
                {
                    if (project.Tasks == null)
                    {
                        project.Tasks = new List<Task>();
                    }
                    Task task = new Task();
                    task.Title = model.Name;
                    task.Description = model.Description;
                    task.Milestone = model.Milestone;
                    task.TaskId = ObjectId.GenerateNewId().ToString();
                    project.Tasks.Add(task);
                    RepositoryContext.Current.Update(project);
                }
                else
                {
                    Task parentTask = null;
                    Task task = new Task();
                    task.Title = model.Name;
                    task.Description = model.Description;
                    task.Milestone = model.Milestone;
                    task.TaskId = ObjectId.GenerateNewId().ToString();
                    parentTask = HandleTreeItems(project.Tasks, model.ParentId);
                    if (parentTask != null)
                    {
                        if (parentTask.SubTask == null)
                        {
                            parentTask.SubTask = new List<Task>();
                        }
                        parentTask.SubTask.Add(task);
                    }
                    else
                    {
                        return View(model);
                    }

                    RepositoryContext.Current.Update(project);
                }

                return RedirectToAction("PlanForProject", "Task", new { @id = model.ProjectId });
            }
            else
            {
                return View(model);
            }
        }

        #endregion

        #region Private Helper

        private Task HandleTreeItems(IEnumerable<Task> nodes, string id)
        {
            Task parentNode = null;

            foreach (Task node in nodes)
            {
                if (node.TaskId == id)
                {
                    parentNode = node;
                    return parentNode;
                }
                if (node.SubTask != null && node.SubTask.Count > 0)
                {
                    parentNode = HandleTreeItems(node.SubTask, id);
                    if (parentNode != null)
                    {
                        return parentNode;
                    }
                }
            }

            return null;
        }
        #endregion
    }
}
