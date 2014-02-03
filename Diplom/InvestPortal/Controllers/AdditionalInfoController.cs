﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Managers;
using BusinessLogic.Notification;
using Invest.Common;
using Invest.Common.Model.Common;
using Invest.Common.Model.Project;
using MongoDB.Bson;

namespace InvestPortal.Controllers
{
	public class AdditionalInfoController : Controller
	{
		#region Fields

		private readonly PortalMessageHandler _portalMessage = new PortalMessageHandler();

		#endregion

		#region Show

		public ActionResult ShowAdditional(AdditionalInfo info)
		{
			return PartialView(info);
		}

		#endregion

		#region Appendix Project

		public ActionResult CreateProjectAdditional(string projectId)
		{
			return PartialView(new AdditionalInfo());
		}

		#endregion

		#region Appendix Notes

		public ActionResult SaveNotes(string noteId)
		{
			var note = RepositoryContext.Current.GetOne<ProjectNotes>(p => p._id == noteId);
			var project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == note.ProjectId);
			var result = new List<AdditionalInfo>();
			if (note.NoteDocument != null)
			{
				result = note.NoteDocument.ToList();
			}
			else
			{
				note.NoteDocument = result;
			}
			var statuses = new List<ViewDataUploadFilesResult>();

			for (int i = 0; i < Request.Files.Count; i++)
			{
				HttpPostedFileBase file = Request.Files[i];
				string fileName = Path.GetFileName(file.FileName);
				string physicalPath = Path.Combine(
					Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/", project.Name)),
					fileName);

				if (!Directory.Exists(
					Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/", project.Name))))
				{
					Directory.CreateDirectory(
						Server.MapPath(string.Format("~/App_Data/ProjectAppendix/{0}/", project.Name)));
				}

				file.SaveAs(physicalPath);
				var document = new DocumentAdditionalInfo();
				document.FilePath = physicalPath;
				document.InfoName = fileName;
				document._id = ObjectId.GenerateNewId().ToString();
				result.Add(document);
				statuses.Add(new ViewDataUploadFilesResult()
				{
					name = fileName,
					size = file.ContentLength,
					type = file.ContentType,
					url = "/AdditionalInfo/DownloadNotesInfo?noteId=" + noteId + "&docId=" + document._id,
					thumbnail_url = @"data:image/png;base64," + EncodeFile(physicalPath),
					delete_type = "GET",
				});
			}

			note.NoteDocument = result;
			RepositoryContext.Current.Update(note);

			return Json(statuses);
		}

		public FileResult DownloadNotesInfo(string noteId, string docId)
		{
			var note = RepositoryContext.Current.GetOne<ProjectNotes>(p => p._id == noteId);
			var doc = note.NoteDocument.FirstOrDefault(t => t._id == docId) as DocumentAdditionalInfo;
			if (doc != null) return File(doc.FilePath, "application/doc", doc.InfoName);
			return null;
		}

		public ActionResult CreateNoteAdditional(string noteId)
		{
			ViewBag.NoteId = noteId;
			ViewBag.SaveUri = string.Format("/AdditionalInfo/SaveNotes?noteId={0}", noteId);
			return PartialView("CreateAdditional");
		}

		#endregion

		#region Appendix Messages

		public ActionResult SaveMessage(string messageId)
		{
			var statuses = new List<ViewDataUploadFilesResult>();
			for (int i = 0; i < Request.Files.Count; i++)
			{
				HttpPostedFileBase file = Request.Files[i];
				var fileName = Path.GetFileName(file.FileName);
				var physicalPath =
					AdditionalInfoManager.GetPhysicalPath(
						"messageAppendix/{1}/{0}/",
						new[]
                        {
                            User.Identity.Name,
                            DateTime.Now.ToString("MM-dd-yy")
                        });

				file.SaveAs(physicalPath + fileName);
				var document = new DocumentAdditionalInfo
				{
					FilePath = physicalPath + fileName,
					InfoName = fileName,
					_id = ObjectId.GenerateNewId().ToString()
				};
				_portalMessage.AddAppendix(
					User.Identity.Name,
					messageId, document);
				statuses.Add(new ViewDataUploadFilesResult()
				{
					name = fileName,
					size = file.ContentLength,
					type = file.ContentType,
					url = "/AdditionalInfo/DownloadNotesInfo?messageId=" + messageId + "&appendixId=" + document._id,
					thumbnail_url = @"data:image/png;base64," + EncodeFile(physicalPath),
					delete_type = "GET",
				});
			}

			return Json(statuses);
		}

		public FileResult DownloadMessageInfo(string messageId, string appendixId)
		{
			var doc = _portalMessage.Appendix(User.Identity.Name, messageId, appendixId) as DocumentAdditionalInfo;
			return doc != null ? File(doc.FilePath, "application/doc", doc.InfoName) : null;
		}


		public ActionResult CreateMessageAdditional(string messageId)
		{
			ViewBag.SaveUri = string.Format("/AdditionalInfo/SaveMessage?messageId={0}", messageId);
			return View("CreateAdditional");
		}

		#endregion

		#region Appendix Report

		public ActionResult CreateReportAdditional(string taskId, string reportId, string projectId)
		{
			ViewBag.SaveUri = string.Format("/AdditionalInfo/SaveReportInfo?taskId={0}&reportId={1}&projectId={2}", taskId, reportId, projectId);
			return View("CreateAdditional");
		}

		public ActionResult SaveReportInfo(string taskId, string reportId, string projectId)
		{
			foreach (HttpPostedFileBase file in Request.Files)
			{
				var statuses = new List<ViewDataUploadFilesResult>();
				var headers = Request.Headers;
				var reportManager = new ReportManager(taskId, reportId, projectId);
				string fileName = headers["X-File-Name"];
				string physicalPath = reportManager.GetReportDocumentPhysicalPath();
				var status = UploadPartialFile(fileName, physicalPath, file);
				reportManager.AddDocumentToReport(
					new DocumentAdditionalInfo
					{
						FilePath = physicalPath + fileName,
						InfoName = fileName,
						_id = ObjectId.GenerateNewId().ToString()
					});

				statuses.Add(status);
			}

			return Content("");
		}

		public FileResult DownloadReportInfo(string taskId, string reportId, string projectId, string docId)
		{
			var reportManager = new ReportManager(taskId, reportId, projectId);
			var doc = reportManager.GetReportAdditionalInfo(docId) as DocumentAdditionalInfo;
			return doc != null ? File(doc.FilePath, "application/doc", doc.InfoName) : null;
		}

		#endregion

		#region Appendix ReviewReport

		public ActionResult SaveReviewReport(string taskId, string reportId, string responsedId, string projectId)
		{
			foreach (HttpPostedFileBase file in Request.Files)
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

		public FileResult DownloadReviewReport(string taskId, string reportId, string responsedId, string projectId)
		{
			var reportManager = new ReportManager(taskId, reportId, projectId);
			var doc = reportManager.GetReportResponseAdditionalInfo(responsedId) as DocumentAdditionalInfo;
			return doc != null ? File(doc.FilePath, "application/doc", doc.InfoName) : null;
		}

		public ActionResult CreateReportReviewAdditional(string taskId, string reportId, string responsedId, string projectId)
		{
			ViewBag.SaveUri = string.Format("/AdditionalInfo/SaveReviewReport?taskId={0}&reportId={1}&projectId={2}&responsedId{3}", taskId, reportId, projectId, responsedId);
			return View("CreateAdditional");
		}

		#endregion

		#region Appendix Test

		[HttpGet]
		public void Delete(string id)
		{
			var filename = id;
			var filePath = Path.Combine(Server.MapPath("~/Files"), filename);

			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}
		}

		[HttpGet]
		public void Download(string id)
		{
			var filename = id;
			var filePath = Path.Combine(Server.MapPath("~/Files"), filename);

			var context = HttpContext;

			if (System.IO.File.Exists(filePath))
			{
				context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
				context.Response.ContentType = "application/octet-stream";
				context.Response.ClearContent();
				context.Response.WriteFile(filePath);
			}
			else
				context.Response.StatusCode = 404;
		}

		[HttpPost]
		public ActionResult UploadFiles(string testId)
		{
			var r = new List<ViewDataUploadFilesResult>();
			for (int i = 0; i < Request.Files.Count; i++)
			{
				var statuses = new List<ViewDataUploadFilesResult>();
				var headers = Request.Headers;
				UploadPartialFile(headers["X-File-Name"], "", Request.Files[i]);
				JsonResult result = Json(statuses);
				result.ContentType = "text/plain";
				return result;
			}

			return Json(r);
		}

		#endregion

		#region Private Helpers

		private string EncodeFile(string fileName)
		{
			return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
		}

		private ViewDataUploadFilesResult UploadPartialFile(string fileName, string storage, HttpPostedFileBase file)
		{
			var inputStream = file.InputStream;
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = file.FileName;
			}
			var fullName = Path.Combine(storage, fileName);
			using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
			{
				var buffer = new byte[1024];
				var l = inputStream.Read(buffer, 0, 1024);
				while (l > 0)
				{
					fs.Write(buffer, 0, l);
					l = inputStream.Read(buffer, 0, 1024);
				}
				fs.Flush();
				fs.Close();
			}

			return new ViewDataUploadFilesResult()
			{
				name = fileName,
				size = file.ContentLength,
				type = file.ContentType,
				url = "/Home/Download/" + fileName,
				thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
				delete_type = "GET",
			};
		}

		#endregion
	}

	#region Nested Class For display Status

	public class ViewDataUploadFilesResult
	{
		public string name { get; set; }
		public int size { get; set; }
		public string type { get; set; }
		public string url { get; set; }
		public string delete_url { get; set; }
		public string thumbnail_url { get; set; }
		public string delete_type { get; set; }
	}

	#endregion
}
