// // -----------------------------------------------------------------------
// // <copyright file="WorkflowActionsController.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Controllers
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using System.Web.Security;
	using Investmogilev.Infrastructure.BusinessLogic.Managers;
	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.State;
	using Investmogilev.UI.Portal.Models;
	using MongoDB.Bson;

	#endregion

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
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name))
						.FillInformation(project);
					break;
				case ProjectWorkflow.Trigger.UpdateInformation:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name))
						.UpdateInformation();
					break;
				case ProjectWorkflow.Trigger.ReOpen:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name))
						.ReOpen();
					break;
				case ProjectWorkflow.Trigger.InvestorResponsed:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name))
						.InvestorResponsed();
					break;
				case ProjectWorkflow.Trigger.InvestorSelected:
					return InvestorSelected(project);
				case ProjectWorkflow.Trigger.DocumentUpdate:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name))
						.DocumentUpdate();
					break;
				case ProjectWorkflow.Trigger.FillInvolvedOrganization:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).FillInvolvedOrganization();
					break;
				case ProjectWorkflow.Trigger.InvolvedOrganizationUpdate:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).InvolvedOrganizationUpdate();
					break;
				case ProjectWorkflow.Trigger.ToComission:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).ToComission();
					break;
				case ProjectWorkflow.Trigger.Comission:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).Comission();
					break;
				case ProjectWorkflow.Trigger.ComissionFix:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).ComissionFix();
					break;
				case ProjectWorkflow.Trigger.ComissionFixUpdate:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).ComissionFixUpdate();
					break;
				case ProjectWorkflow.Trigger.ToIspolcom:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).ToIspolcom();
					break;
				case ProjectWorkflow.Trigger.Ispolcom:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).Ispolcom();
					break;
				case ProjectWorkflow.Trigger.ToIspolcomFix:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).ToIspolcomFix();
					break;
				case ProjectWorkflow.Trigger.IspolcomFixUpdate:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).IspolcomFixUpdate();
					break;
				case ProjectWorkflow.Trigger.ToMinEconomy:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).ToMinEconomy();
					break;
				case ProjectWorkflow.Trigger.MinEconomyResponsed:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).MinEconomyResponsed();
					break;
				case ProjectWorkflow.Trigger.UpdatePlan:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).UpdatePlan();
					break;
				case ProjectWorkflow.Trigger.ApprovePlan:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).ApprovePlan();
					break;
				case ProjectWorkflow.Trigger.UpdateRealization:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).UpdateRealization();
					break;
				case ProjectWorkflow.Trigger.RejectDocument:
					ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
						Roles.GetRolesForUser(User.Identity.Name)).RejectDocument();
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
			return RedirectToAction("Project", "BaseProject", new { id = project._id });
		}


		public ActionResult RemoveResponse(string projectId, string responseId)
		{
			var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
			project.Responses.Remove(project.Responses.Find(t => t.ResponseId == responseId));
			if (project.Responses.Count == 0)
			{
				project.WorkflowState.CurrentState = ProjectWorkflow.State.OnMap;
			}
			RepositoryContext.Current.Update(project);

			return RedirectToAction("Project", "BaseProject", new { id = projectId });
		}

		public ActionResult SelectUser(string projectId, string responseId)
		{
			var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
			IEnumerable<InvestorResponse> prevResponses = project.Responses.Where(r => r.IsVerified);
			foreach (var prevResponse in prevResponses)
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
					var document in project.Tasks.Where(t => t.Step == ProjectWorkflow.State.DocumentSending))
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

			foreach (var template in templates)
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
				foreach (var document in project.Tasks.Where(t => t.Type == TaskTypes.InvolvedOrganiztion))
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

			foreach (var template in templates)
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
			try
			{
				ProjectStateManager.StateManagerFactory(project, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).DocumentUpdate();
			}
			catch (Exception)
			{
			}
			return RedirectToAction("Project", "BaseProject", new {id = model.PorjectId});
		}

		public ActionResult FillInvolvedOrganization(Project project)
		{
			return RedirectToAction("Project", "BaseProject", new {id = project._id});
		}

		#endregion

		#region Comission Fixes

		public ActionResult AddFix(string projectid, ProjectWorkflow.State state)
		{
			return View(new ProjectTask
			{
				_id = ObjectId.GenerateNewId().ToString(),
				CreationTime = DateTime.Now,
				ProjectId = projectid,
				Step = state
			});
		}

		[HttpPost]
		public ActionResult AddFix(ProjectTask projectTask)
		{
			if (!ModelState.IsValid)
			{
				return View(projectTask);
			}

			if (projectTask.Milestone < DateTime.Now)
			{
				ModelState.AddModelError("Milestone", "Дата выполнения исправлений не может быть в прошлом");
				return View(projectTask);
			}

			var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectTask.ProjectId);
			if (project.Tasks == null)
			{
				project.Tasks = new List<ProjectTask>();
			}

			project.Tasks.Add(projectTask);
			RepositoryContext.Current.Update(project);

			return RedirectToAction("Project", "BaseProject", new {id = projectTask.ProjectId});
		}

		#endregion
	}
}