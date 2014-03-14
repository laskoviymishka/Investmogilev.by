using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Mvc;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Repository;
using Investmogilev.Infrastructure.Common.State;
using Newtonsoft.Json;

namespace Investmogilev.UI.Portal.Controllers
{
	using System.Linq;

	public class InvestProjectsController : Controller
	{
		#region Nested class

		private class ViewModel
		{
			public List<LatLng> Data { get; set; }
			public int Take { get; set; }
			public int Page { get; set; }
			public int Total { get; set; }
		}

		private class LatLng
		{
			public double? Lat { get; set; }
			public double? Lng { get; set; }
			public string Name { get; set; }
			public string _id { get; set; }
			public bool Perechen { get; set; }
			public string Description { get; set; }
			public string Type { get; set; }
			public List<string> Tags { get; set; }
		}

		#endregion

		#region External method

		[AllowAnonymous]
		public ActionResult PopUpDetailsGreenField(string id)
		{
			var project = RepositoryContext.Current.GetOne<Project>(p => p._id == id) as GreenField;

			if (project == null)
			{
				return null;
			}

			return PartialView(project);
		}

		[AllowAnonymous]
		public ActionResult PopUpDetailsUnUsedBuilding(string id)
		{
			var project = RepositoryContext.Current.GetOne<Project>(p => p._id == id) as UnUsedBuilding;
			if (project == null)
			{
				return null;
			}

			return PartialView(project);
		}

		[AllowAnonymous]
		public ActionResult PopUpDetailsTemplate(string id)
		{
			var project = RepositoryContext.Current.GetOne<Project>(p => p._id == id) as Template;
			if (project == null)
			{
				return null;
			}

			return PartialView(project);
		}

		[AllowAnonymous]
		public string ProjectGeoJson(int take, int page = 0)
		{
			var returnModel = new ViewModel();
			returnModel.Total = RepositoryContext.Current.All<Project>(
				p => p.WorkflowState.CurrentState == ProjectWorkflow.State.OnMap
					 || p.WorkflowState.CurrentState == ProjectWorkflow.State.InvestorApprove).Count();
			returnModel.Take = take;
			returnModel.Page = page;
			returnModel.Data = GenerateGeoJsonData(RepositoryContext.Current.All<Project>(
				p => p.WorkflowState.CurrentState == ProjectWorkflow.State.OnMap
					 || p.WorkflowState.CurrentState == ProjectWorkflow.State.InvestorApprove).Skip(page * take).Take(take));

			return JsonConvert.SerializeObject(returnModel);
		}

		[AllowAnonymous]
		public ActionResult PartialTable()
		{
			ViewBag.StartYear = 2005;
			ViewBag.EndYear = 2012;
			IRepository repository = new MongoRepository(WebConfigurationManager.AppSettings["mongoServer"],
				WebConfigurationManager.AppSettings["mongoBase"]);
			return PartialView(repository.All<Region>());
		}

		#endregion

		#region Private Helpers

		private List<LatLng> GenerateGeoJsonData(IEnumerable<Project> projects)
		{
			List<LatLng> latLngs = new List<LatLng>();
			foreach (Project project in projects)
			{
				if (project.Address.Lat != 1)
				{
					var latLng = new LatLng
					{
						Lat = project.Address.Lat,
						Lng = project.Address.Lng,
						Name = project.Name,
						Description = project.Description,
						_id = project._id,
						Type = project.GetType().Name,
						Perechen = project.IsInList,
						Tags = new List<string>()
					};
					latLng.Tags.Add(project.Tag.ToString());
					latLngs.Add(latLng);
				}
			}

			return latLngs;
		}

		#endregion
	}
}