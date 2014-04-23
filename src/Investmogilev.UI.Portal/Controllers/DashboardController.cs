// // -----------------------------------------------------------------------
// // <copyright file="DashboardController.cs"  company="Andrei Tserakhau">
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
	using Investmogilev.Infrastructure.BusinessLogic.Managers;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Newtonsoft.Json;

	#endregion

	public class DashboardController : Controller
	{
		private readonly StatisticsManager _statistica = new StatisticsManager();

		public ActionResult Statistica()
		{
			return View(_statistica.GetAllSessions(DateTime.MinValue, DateTime.Now));
		}

		public JsonResult GetOverallStatistic()
		{
			var model = _statistica.GetAllSessions(DateTime.MinValue, DateTime.Now).OrderBy(r => r.Time);
			List<OverallStatistic> statistics = new List<OverallStatistic>();
			foreach (var statSession in model)
			{
				if (statistics.Any(t => t.Date == statSession.Time.ToShortDateString()))
				{
					statistics.Find(t => t.Date == statSession.Time.ToShortDateString()).Count++;
				}
				else
				{
					statistics.Add(new OverallStatistic { Date = statSession.Time.ToShortDateString(), Count = 1 });
				}
			}
			return Json(statistics.ToArray(), JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetMostViewedProjects()
		{
			var model = _statistica.GetAllSessions(DateTime.MinValue, DateTime.Now).OrderBy(r => r.Time);
			var statistics = new List<MostViewedProjects>();
			foreach (var statSession in model)
			{
				foreach (var activity in statSession.Activities)
				{
					if (activity.Type == ActivityType.OpenGreenfield || activity.Type == ActivityType.OpenTemplate ||
						activity.Type == ActivityType.OpenUnUsedBuildg)
					{
						if (statistics.Any(s => s.ProjectId == activity.RefferenceId))
						{
							var item = statistics.Find(s => s.ProjectId == activity.RefferenceId);
							item.Count++;
							item.LastView = statSession.Time.ToShortDateString();
						}
						else
						{
							statistics.Add(new MostViewedProjects
							{
								Count = 1,
								LastView = statSession.Time.ToShortDateString(),
								ProjectId = activity.RefferenceId,
								ProjectType = activity.Type.ToString()
							});
						}
					}
				}
			}
			return Json(statistics.ToArray(), JsonRequestBehavior.AllowGet);
		}


		#region Private Classes

		private class OverallStatistic
		{
			public string Date;
			public int Count;
		}

		private class MostViewedProjects
		{
			public string ProjectId { get; set; }
			public string ProjectType { get; set; }
			public int Count { get; set; }
			public string LastView { get; set; }
		}

		#endregion
	}
}