using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common;
using Invest.Common.Model.Project;
using Invest.Common.State;
using InvestPortal.Models;
using MongoDB.Bson;

namespace InvestPortal.Controllers
{
    public class WorkflowActionsController : Controller
    {
        #region Common methods

        public ActionResult StatusInfo(string id)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == id);
            if (project == null)
            {
                return HttpNotFound("проект не найден");
            }

            ViewBag.AvaibleTriggers = ProjectStateManager.StateManagerFactory(
                project,
                User.Identity.Name,
                Roles.GetRolesForUser(User.Identity.Name))
                .GetAvaibleTriggers();

            return PartialView(project.WorkflowState.CurrentState.ToString(), project);
        }

        public ActionResult Triggers(string projectId, string trigger)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            if (project == null)
            {
                return HttpNotFound("проект не найден");
            }
            ProjectWorkflow.Trigger triggerEnum;
            Enum.TryParse(trigger, out triggerEnum);
            switch (triggerEnum)
            {
                case ProjectWorkflow.Trigger.FillInformation:
                    break;
                case ProjectWorkflow.Trigger.UpdateInformation:
                    break;
                case ProjectWorkflow.Trigger.ReOpen:
                    break;
                case ProjectWorkflow.Trigger.InvestorResponsed:
                    break;
                case ProjectWorkflow.Trigger.InvestorSelected:
                    return RedirectToAction("InvestorSelected", project);
                case ProjectWorkflow.Trigger.DocumentUpdate:
                    return View("DocumentUpdate",
                        project.Tasks.Where(t => t.Step == ProjectWorkflow.State.DocumentSending));
                case ProjectWorkflow.Trigger.FillInvolvedOrganization:
                    ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
                        Roles.GetRolesForUser(User.Identity.Name)).FillInvolvedOrganization();
                    return RedirectToAction("Project", "BaseProject", new {id = project._id});
                case ProjectWorkflow.Trigger.InvolvedOrganizationUpdate:
                    ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
                        Roles.GetRolesForUser(User.Identity.Name)).InvolvedOrganizationUpdate();
                    break;
                case ProjectWorkflow.Trigger.ToComission:
                    ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
                        Roles.GetRolesForUser(User.Identity.Name)).ToComission();
                    break;
                case ProjectWorkflow.Trigger.Comission:
                    break;
                case ProjectWorkflow.Trigger.ComissionFix:
                    break;
                case ProjectWorkflow.Trigger.ComissionFixUpdate:
                    break;
                case ProjectWorkflow.Trigger.ToIspolcom:
                    break;
                case ProjectWorkflow.Trigger.Ispolcom:
                    break;
                case ProjectWorkflow.Trigger.ToIspolcomFix:
                    break;
                case ProjectWorkflow.Trigger.IspolcomFixUpdate:
                    break;
                case ProjectWorkflow.Trigger.ToMinEconomy:
                    break;
                case ProjectWorkflow.Trigger.MinEconomyResponsed:
                    break;
                case ProjectWorkflow.Trigger.UpdatePlan:
                    break;
                case ProjectWorkflow.Trigger.ApprovePlan:
                    break;
                case ProjectWorkflow.Trigger.UpdateRealization:
                    break;
                case ProjectWorkflow.Trigger.RejectDocument:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return RedirectToAction("Project", "BaseProject", new {id = project._id});
        }

        #endregion

        #region InvestorSelected

        public ActionResult InvestorSelected(Project project)
        {
            ProjectStateManager
                .StateManagerFactory(
                    project,
                    User.Identity.Name,
                    Roles.GetRolesForUser(User.Identity.Name))
                .InvestorSelected();
            return View(project);
        }


        public ActionResult SelectUser(string projectId, string responseId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            IEnumerable<InvestorResponse> prevResponses = project.Responses.Where(r => r.IsVerified);
            foreach (InvestorResponse prevResponse in prevResponses)
            {
                prevResponse.IsVerified = false;
            }
            InvestorResponse response = project.Responses.Find(r => r.ResponseId == responseId);
            response.IsVerified = true;
            RepositoryContext.Current.Update(project);

            return RedirectToAction("Project", "BaseProject", new {id = projectId});
        }

        public ActionResult AddDocumentTask(string projectId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            if (project == null)
            {
                return HttpNotFound("проект не найден");
            }

            var model = new AddDocumentViewModel();
            model.PorjectId = projectId;
            model.Documents = new List<string>();

            ViewBag.DocumentList =
                RepositoryContext.Current.All<TaskTemplate>(t => t.Step == ProjectWorkflow.State.DocumentSending)
                    .Select(template => template.Title)
                    .ToList();

            if (project.Tasks != null)
            {
                foreach (
                    ProjectTask document in project.Tasks.Where(t => t.Step == ProjectWorkflow.State.DocumentSending))
                {
                    model.Documents.Add(document.Title);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddDocumentTask(AddDocumentViewModel model)
        {
            ViewBag.DocumentList =
                RepositoryContext.Current.All<TaskTemplate>(t => t.Step == ProjectWorkflow.State.DocumentSending)
                    .Select(template => template.Title)
                    .ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == model.PorjectId);

            IQueryable<TaskTemplate> templates =
                RepositoryContext.Current.All<TaskTemplate>(t => model.Documents.Contains(t.Title));
            var tasks = new List<ProjectTask>();

            foreach (TaskTemplate template in templates)
            {
                tasks.Add(new ProjectTask
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    ProjectId = model.PorjectId,
                    Body = template.Body,
                    Title = template.Title,
                    Type = template.Type,
                    Step = template.Step,
                    CreationTime = DateTime.Now
                });
            }

            if (project.Tasks == null)
            {
                project.Tasks = new List<ProjectTask>();
            }
            project.Tasks.RemoveAll(p => p.Step == ProjectWorkflow.State.DocumentSending);
            project.Tasks.AddRange(tasks);

            RepositoryContext.Current.Update(project);

            return RedirectToAction("Project", "BaseProject", new {id = model.PorjectId});
        }

        #endregion

        #region InvolvedOrganization

        public ActionResult AddInvolvedOrganizations(string projectId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            if (project == null)
            {
                return HttpNotFound("проект не найден");
            }

            var model = new AddDocumentViewModel();
            model.PorjectId = projectId;
            model.Documents = new List<string>();

            ViewBag.DocumentList =
                RepositoryContext.Current.All<TaskTemplate>(t => t.Type == TaskTypes.InvolvedOrganiztion)
                    .Select(template => template.Title)
                    .ToList();

            if (project.Tasks != null)
            {
                foreach (ProjectTask document in project.Tasks.Where(t => t.Type == TaskTypes.InvolvedOrganiztion))
                {
                    model.Documents.Add(document.Title);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddInvolvedOrganizations(AddDocumentViewModel model)
        {
            ViewBag.DocumentList =
                RepositoryContext.Current.All<TaskTemplate>(t => t.Type == TaskTypes.InvolvedOrganiztion)
                    .Select(template => template.Title)
                    .ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == model.PorjectId);

            IQueryable<TaskTemplate> templates =
                RepositoryContext.Current.All<TaskTemplate>(t => model.Documents.Contains(t.Title));
            var tasks = new List<ProjectTask>();

            foreach (TaskTemplate template in templates)
            {
                tasks.Add(new ProjectTask
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    ProjectId = model.PorjectId,
                    Body = template.Body,
                    Title = template.Title,
                    Type = template.Type,
                    Step = ProjectWorkflow.State.InvolvedOrganizations,
                    CreationTime = DateTime.Now
                });
            }

            if (project.Tasks == null)
            {
                project.Tasks = new List<ProjectTask>();
            }
            project.Tasks.RemoveAll(p => p.Type == TaskTypes.InvolvedOrganiztion);
            project.Tasks.AddRange(tasks);

            RepositoryContext.Current.Update(project);
            ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
                Roles.GetRolesForUser(User.Identity.Name)).DocumentUpdate();
            return RedirectToAction("Project", "BaseProject", new {id = model.PorjectId});
        }

        public ActionResult FillInvolvedOrganization(Project project)
        {
            return RedirectToAction("Project", "BaseProject", new {id = project._id});
        }

        #endregion
    }
}