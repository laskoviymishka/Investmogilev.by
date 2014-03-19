// // -----------------------------------------------------------------------
// // <copyright file="Global.asax.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal
{
	#region Using

	using System;
	using System.Web;
	using System.Web.Http;
	using System.Web.Mvc;
	using System.Web.Routing;
	using NLog;
	using StackExchange.Profiling;

	#endregion

	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"TaskInfo",
				"Task/{action}/{taskId}/{projectId}/{infoId}",
				new {controller = "Task"}
				);

			routes.MapRoute(
				"TaskOther",
				"Task/{action}/{taskId}/{projectId}",
				new {controller = "Task"}
				);

			routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
				);

			routes.MapRoute("Default", "{controller}/{action}/{id}",
				new {controller = "Home", action = "Index", id = UrlParameter.Optional}
				);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			ModelBinders.Binders.Add(typeof (DateTime), new DateTimeBinder());
			ModelBinders.Binders.Add(typeof (double), new DoubleBinder());
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
			Logger.Info("Application Start");
		}

		protected void Application_BeginRequest()
		{
			MiniProfiler.Start();
			var log = string.Format("{0}, Request.UrlReferrer: {1}, Request.HttpMethod: {2}",
				"Application_BeginRequest",
				Request.UrlReferrer,
				Request.HttpMethod);
			Logger.Info(log);
		}

		protected void Application_EndRequest()
		{
			MiniProfiler.Stop();
		}
	}
}