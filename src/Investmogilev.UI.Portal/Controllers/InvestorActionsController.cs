﻿// // -----------------------------------------------------------------------
// // <copyright file="InvestorActionsController.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Controllers
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using Investmogilev.Infrastructure.BusinessLogic.Managers;
	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.State;
	using MongoDB.Bson;

	#endregion

	[Authorize(Roles = "Investor")]
	public class InvestorActionsController : Controller
	{
		#region MainActions

		public ActionResult Index()
		{
			IQueryable<Project> projects =
				RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name);
			var model = new List<ProjectTask>();
			foreach (var project in projects)
			{
				foreach (var task in project.Tasks)
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
			foreach (var project in projects)
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

		#region Map Point

		public ActionResult AddRoadMapPoint(string projectid)
		{
			return View(new ProjectTask
			{
				_id = ObjectId.GenerateNewId().ToString(),
				CreationTime = DateTime.Now,
				ProjectId = projectid,
				Type = TaskTypes.Investor,
				Step = ProjectWorkflow.State.Realization
			});
		}

		[HttpPost]
		public ActionResult AddRoadMapPoint(ProjectTask projectTask)
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