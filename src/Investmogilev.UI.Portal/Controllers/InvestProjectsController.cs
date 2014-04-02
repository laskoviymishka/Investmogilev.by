// // -----------------------------------------------------------------------
// // <copyright file="InvestProjectsController.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Controllers
{
	#region Using

	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.Repository;
	using Investmogilev.Infrastructure.Common.State;
	using Investmogilev.UI.Portal.Models;
	using Newtonsoft.Json;

	#endregion

	public class InvestProjectsController : Controller
	{
		private static PartialTableViewModel CacheViewModel;

		#region Nested class

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

		private class ViewModel
		{
			public List<LatLng> Data { get; set; }
			public int Take { get; set; }
			public int Page { get; set; }
			public int Total { get; set; }
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
			if (CacheViewModel == null)
			{
				IRepository repository = new MongoRepository("mongodb://tserakhau.cloudapp.net", "Projects");
				var regions = repository.All<Region>().ToList();
				CacheViewModel = GenerateViewModel(regions);
			}
			return PartialView(CacheViewModel);
		}

		private class TempModel
		{
			public string name { get; set; }
			public List<TempParametr> Data { get; set; }
		}

		private class TempParametr
		{
			public int Year { get; set; }
			public List<double> Values { get; set; }
		}

		private PartialTableViewModel GenerateViewModel(List<Region> regions)
		{
			var allGrowth = new Dictionary<string, List<double>>();
			var result = new PartialTableViewModel();
			result.Regions = new List<RegionDataViewModel>();
			result.AveragesParametrGrowth = new List<AvgParametr>();
			var tempModel = new List<TempModel>();

			foreach (var item in regions)
			{
				var region = new RegionDataViewModel();
				region.RegionName = item.RegionName;
				region.Parametrs = new List<ParametrViewModel>();
				foreach (var inner in item.Parametrs)
				{
					foreach (var child in inner.ChildParametrs)
					{
						if (tempModel.All(t => t.name != child.ParametrName))
						{
							tempModel.Add(new TempModel() { name = child.ParametrName, Data = new List<TempParametr>() });
						}

						var tempItem = tempModel.First(t => t.name == child.ParametrName);

						var avgGrowth = 0.0;
						var count = 0;
						var par = new ParametrViewModel();
						par.Name = child.ParametrName;
						par.ParentParametrName = inner.ParametrName;
						par.Data = new Dictionary<int, double>();
						par.Growthes = new Dictionary<int, double>();
						foreach (var pair in child.Values)
						{
							par.Data.Add(pair.Key, pair.Value);
						}

						double growth = 0.0;
						for (int i = 1; i < child.Values.Count; i++)
						{
							if (child.Values[i].Value != 0.0 && child.Values[i - 1].Value != 0.0)
							{
								growth += child.Values[i].Value / child.Values[i - 1].Value;
								par.Growthes.Add(child.Values[i].Key, child.Values[i].Value / child.Values[i - 1].Value);
								if (tempItem.Data.All(t => t.Year != child.Values[i].Key))
								{
									tempItem.Data.Add(new TempParametr() { Year = child.Values[i].Key, Values = new List<double>() });
								}
								tempItem.Data.First(t => t.Year == child.Values[i].Key).Values.Add(child.Values[i].Value / child.Values[i - 1].Value);
								count++;
							}
						}
						par.Growth = growth / count;


						if (count != 0)
						{
							avgGrowth = growth / count;
						}
						if (!allGrowth.ContainsKey(child.ParametrName))
						{
							allGrowth.Add(child.ParametrName, new List<double>());
						}
						if (avgGrowth != 0.0)
						{
							allGrowth[child.ParametrName].Add(avgGrowth);
						}
						region.Parametrs.Add(par);
					}
				}
				result.Regions.Add(region);
			}
			result.AveragesTotal = new List<KeyValuePair<string, double>>();
			foreach (var pair in allGrowth)
			{
				result.AveragesTotal.Add(new KeyValuePair<string, double>(pair.Key, pair.Value.Average()));
			}
			foreach (var model in tempModel)
			{
				var avgPar = new AvgParametr();
				avgPar.Name = model.name;
				avgPar.Data = new Dictionary<int, double>();
				foreach (var data in model.Data)
				{
					avgPar.Data.Add(data.Year, data.Values.Average());
				}
				result.AveragesParametrGrowth.Add(avgPar);
			}
			return result;
		}

		#endregion

		#region Private Helpers

		private List<LatLng> GenerateGeoJsonData(IEnumerable<Project> projects)
		{
			var latLngs = new List<LatLng>();
			foreach (var project in projects)
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