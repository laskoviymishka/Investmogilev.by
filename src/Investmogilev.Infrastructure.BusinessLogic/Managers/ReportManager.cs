// // -----------------------------------------------------------------------
// // <copyright file="ReportManager.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Managers
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Model.Project;

	#endregion

	public class ReportManager
	{
		#region Fields

		private const string ReportTemplateFilePath = "/ProjectInfo/{0}/Tasks/{1}/TaskReports/{2}";
		private const string ResponseTemplateFilePath = "/ProjectInfo/{0}/Tasks/{1}/TaskReports/{2}/response/";
		private readonly Project _currentProject;
		private readonly ProjectTask _currentTask;
		private readonly string _projectId;
		private readonly string _reportId;
		private readonly string _taskId;
		private Report _currentReport;

		#endregion

		#region Constructor

		public ReportManager(string taskId, string reportId, string projectId)
		{
			_taskId = taskId;
			_reportId = reportId;
			_projectId = projectId;
			_currentProject = RepositoryContext.Current.GetOne<Project>(p => p._id == _projectId);
			_currentTask = _currentProject.Tasks.Find(t => t._id == _taskId);
			if (_currentTask.TaskReport == null || !_currentTask.TaskReport.Any())
			{
				_currentTask.TaskReport = new List<Report>();
			}
		}

		#endregion

		#region Properties

		public Project CurrentProject
		{
			get { return _currentProject; }
		}

		public ProjectTask CurrentTask
		{
			get { return _currentTask; }
		}

		public Report CurrentReport
		{
			get { return _currentReport; }
		}

		#endregion

		#region Report Actions

		public void CreateReport(Report report)
		{
			_currentReport = _currentTask.TaskReport.Find(t => t._id == report._id);
			if (_currentReport != null)
			{
				report = _currentTask.TaskReport.Find(t => t._id == report._id);
				report.Body = report.Body;
				report.ReportTime = report.ReportTime;
			}
			else
			{
				_currentReport = report;
				_currentTask.TaskReport.Add(_currentReport);
			}

			RepositoryContext.Current.Update(_currentProject);
		}


		public void AddDocumentToReport(AdditionalInfo document)
		{
			if (_currentTask.TaskReport.Find(t => t._id == _reportId) != null)
			{
				_currentReport = _currentTask.TaskReport.Find(t => t._id == _reportId);
			}
			else
			{
				_currentReport = new Report {ProjectId = _projectId, _id = _reportId, TaskId = _taskId};
				_currentTask.TaskReport.Add(_currentReport);
			}

			if (_currentReport.Info == null || !_currentReport.Info.Any())
			{
				_currentReport.Info = new List<AdditionalInfo>();
			}

			_currentReport.Info.Add(document);
			RepositoryContext.Current.Update(_currentProject);
		}

		public string GetReportDocumentPhysicalPath()
		{
			return AdditionalInfoManager.GetPhysicalPath(
				ReportTemplateFilePath,
				new[]
				{
					_currentProject.Name,
					_currentTask.Title,
					DateTime.Now.ToString("MM-dd-yy")
				});
		}

		public List<AdditionalInfo> GetReportAdditionalInfos()
		{
			return _currentReport.Info;
		}

		public AdditionalInfo GetReportAdditionalInfo(string infoId)
		{
			return _currentReport.Info.Find(i => i._id == infoId);
		}

		#endregion

		#region Response Actions

		public void CreateReportResponse(ReportResponse reportResponse)
		{
			_currentReport = _currentTask.TaskReport.Find(t => t._id == reportResponse.ReportId);

			if (_currentReport.ReportResponse == null)
			{
				_currentReport.ReportResponse = reportResponse;
			}
			else
			{
				_currentReport.ReportResponse.ResponseTime = reportResponse.ResponseTime;
				_currentReport.ReportResponse.Body = reportResponse.Body;
				_currentReport.ReportResponse.IsApproved = reportResponse.IsApproved;
			}

			if (_currentReport.ReportResponse.IsApproved)
			{
				_currentTask.IsComplete = true;
				_currentTask.CompleteTime = DateTime.Now;
			}

			RepositoryContext.Current.Update(_currentProject);
		}

		public void AddDocumentToReportResponse(string reposponseId, AdditionalInfo document)
		{
			if (_currentTask.TaskReport.Find(t => t._id == _reportId) != null)
			{
				_currentReport = _currentTask.TaskReport.Find(t => t._id == _reportId);
			}
			else
			{
				throw new InvalidOperationException("не могу добавить ответ на отчет к несуществующему отчету");
			}

			if (_currentReport.ReportResponse == null)
			{
				_currentReport.ReportResponse = new ReportResponse
				{
					_id = reposponseId,
					ProjectId = _projectId,
					TaskId = _taskId,
					ReportId = _reportId,
					Info = new List<AdditionalInfo>()
				};
			}

			_currentReport.Info.Add(document);
			RepositoryContext.Current.Update(_currentProject);
		}

		public string GetReportResponseDocumentPhysicalPath()
		{
			return AdditionalInfoManager.GetPhysicalPath(
				ResponseTemplateFilePath,
				new[]
				{
					_currentProject.Name,
					_currentTask.Title,
					DateTime.Now.ToString("MM-dd-yy")
				});
		}

		public List<AdditionalInfo> GetReportResponseAdditionalInfos()
		{
			return _currentReport.ReportResponse.Info;
		}

		public AdditionalInfo GetReportResponseAdditionalInfo(string infoId)
		{
			return _currentReport.ReportResponse.Info.Find(i => i._id == infoId);
		}

		#endregion
	}
}