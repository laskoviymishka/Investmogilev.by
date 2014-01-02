using System.Collections.Generic;
using System.Web.Mvc;
using Invest.Common;
using Invest.Common.Model.Common;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Newtonsoft.Json;

namespace InvestPortal.Controllers
{
    [Authorize]
    public class InvestProjectsController : Controller
    {
        #region Nested class

        class LatLng
        {
            public double? Lat { get; set; }
            public double? Lng { get; set; }
            public string Name { get; set; }
            public string _id { get; set; }
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
        public string ProjectGeoJson()
        {
            return JsonConvert.SerializeObject(GenerateGeoJsonData(RepositoryContext.Current.All<Project>()));
        }

		[AllowAnonymous]
		public ActionResult PartialTable()
		{
			ViewBag.StartYear = 2005;
			ViewBag.EndYear = 2012;
			IRepository repository = new MongoRepository("mongodb://tserakhau.cloudapp.net", "Projects");
			return PartialView(repository.All<Region>());
		}

        #endregion

        #region Private Helpers

        private IList<LatLng> GenerateGeoJsonData(IEnumerable<Project> projects)
        {
            IList<LatLng> latLngs = new List<LatLng>();
            foreach (var project in projects)
            {
                if (project.Address.Lat != 1)
                {
                    latLngs.Add(new LatLng()
                    {
                        Lat = project.Address.Lat,
                        Lng = project.Address.Lng,
                        Name = project.Name,
                        Description = project.Description,
                        _id = project._id,
                        Type = project.GetType().Name,
                        Tags = project.Tags
                    });
                }
            }

            return latLngs;
        }

        #endregion
    }
}