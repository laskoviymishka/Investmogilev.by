using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
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
        public WorkflowActionsController()
        {
        }

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

            switch (project.WorkflowState.CurrentState)
            {
                case ProjectWorkflow.State.Open:
                    return PartialView("Open", project);
                case ProjectWorkflow.State.OnMap:
                    return PartialView("OnMap", project);
                case ProjectWorkflow.State.InvestorApprove:
                    return PartialView("InvestorApprove", project);
                case ProjectWorkflow.State.DocumentSending:
                    return PartialView("DocumentSending", project);
                case ProjectWorkflow.State.WaitInvolved:
                    return PartialView("WaitInvolved", project);
                case ProjectWorkflow.State.InvolvedOrganizations:
                    return PartialView("InvolvedOrganizations", project);
                case ProjectWorkflow.State.WaitComission:
                    return PartialView("WaitComission", project);
                case ProjectWorkflow.State.OnComission:
                    return PartialView("OnComission", project);
                case ProjectWorkflow.State.WaitComissionFixes:
                    return PartialView("WaitComissionFixes", project);
                case ProjectWorkflow.State.WaitIspolcom:
                    return PartialView("WaitIspolcom", project);
                case ProjectWorkflow.State.OnIspolcom:
                    return PartialView("OnIspolcom", project);
                case ProjectWorkflow.State.WaitIspolcomFixes:
                    return PartialView("WaitIspolcomFixes", project);
                case ProjectWorkflow.State.InMinEconomy:
                    return PartialView("InMinEconomy", project);
                case ProjectWorkflow.State.PlanCreating:
                    return PartialView("PlanCreating", project);
                case ProjectWorkflow.State.Realization:
                    return PartialView("Realization", project);
                case ProjectWorkflow.State.Done:
                    return PartialView("Done", project);
                default:
                    return HttpNotFound("не известное состояние проекта");
            }
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
                    return RedirectToAction("DocumentUpdate", project);
                case ProjectWorkflow.Trigger.FillInvolvedOrganization:
                    return RedirectToAction("FillInvolvedOrganization", project);
                case ProjectWorkflow.Trigger.InvolvedOrganizationUpdate:
                    break;
                case ProjectWorkflow.Trigger.ToComission:
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

            return null;
        }

        #endregion

        public ActionResult InvestorSelected(Project project)
        {
            return View(project);
        }

        public ActionResult DocumentUpdate(Project project)
        {
            return View(project);
        }

        public ActionResult FillInvolvedOrganization(Project project)
        {
            return View(project);
        }

        public ActionResult SelectUser(string projectId, string responseId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            var prevResponses = project.Responses.Where(r => r.IsVerified);
            foreach (var prevResponse in prevResponses)
            {
                prevResponse.IsVerified = false;
            }
            var response = project.Responses.Find(r => r.ResponseId == responseId);
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

            ViewBag.DocumentList = RepositoryContext.Current.All<TaskTemplate>(t => t.Step == ProjectWorkflow.State.DocumentSending).Select(template => template.Title).ToList();

            if (project.Tasks != null)
            {
                foreach (var document in project.Tasks.Where(t => t.Step == ProjectWorkflow.State.DocumentSending))
                {
                    model.Documents.Add(document.Title);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddDocumentTask(AddDocumentViewModel model)
        {
            ViewBag.DocumentList = RepositoryContext.Current.All<TaskTemplate>(t => t.Step == ProjectWorkflow.State.DocumentSending).Select(template => template.Title).ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == model.PorjectId);

            var templates = RepositoryContext.Current.All<TaskTemplate>(t => model.Documents.Contains(t.Title));

            var tasks = templates.Select(template => new ProjectTask()
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    Body = template.Body,
                    Title = template.Title,
                    Type = template.Type,
                    Step = template.Step,
                    CreationTime = DateTime.Now
                }).ToList();

            if (project.Tasks == null)
            {
                project.Tasks = new List<ProjectTask>();
            }
            project.Tasks.RemoveAll(p => p.Step == ProjectWorkflow.State.DocumentSending);
            project.Tasks.AddRange(tasks);

            RepositoryContext.Current.Update(project);

            return RedirectToAction("Project", "BaseProject", new { id = model.PorjectId });
        }
    }
}
