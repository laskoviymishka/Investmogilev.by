using System.Collections.Generic;
using System.Web.Mvc;
using Invest.Common.Model.Common;
using Invest.Common.Model.ProjectModels;
using Invest.Common.Repository;
using Invest.Common.State;
using MongoRepository;
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
        }

        #endregion

        #region Fields

        private readonly ProjectRepository _repository = new ProjectRepository();

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
        public ActionResult PopUpDetailsBrownField(string id)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == id) as BrownField;

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
            return JsonConvert.SerializeObject(GenerateGeoJsonData(RepositoryContext.Current.All<Project>(p => p.WorkflowState.CurrenState == ProjectStates.WaitForInvestor
                || p.WorkflowState.CurrenState == ProjectStates.WaitForAdminInvestorApprove)));
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
                        Type = project.GetType().Name
                    });
                }
            }

            return latLngs;
        }

        [AllowAnonymous]
        public ActionResult PartialTable()
        {
            ViewBag.StartYear = 2005;
            ViewBag.EndYear = 2012;
            return PartialView(RepositoryContext.Current.All<Region>());
        }

        #endregion
    }
}