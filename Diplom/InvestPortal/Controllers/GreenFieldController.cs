using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invest.Common.Model;
using Invest.Common.Repository;
using InvestPortal.Models;
using MongoRepository;
using Telerik.Web.Mvc;

namespace InvestPortal.Controllers
{
    public class GreenFieldController : Controller
    {
        #region Private Fields

        private readonly ProjectRepository _repository;
        private readonly IRepository _mongoRepository;

        #endregion

        #region Constructor

        public GreenFieldController()
        {
            _repository = new ProjectRepository();
            _mongoRepository = RepositoryContext.Current;
        }

        #endregion

        #region Main Methods

        public ActionResult Index()
        {
            ViewBag.Users = new List<BaseProjectController.NestedUserViewModel>();
            ViewBag.Regions = new List<BaseProjectController.NestedRegionViewModel>();
            foreach (Users mongoUser in _mongoRepository.All<Users>())
            {
                ViewBag.Users.Add(new BaseProjectController.NestedUserViewModel() { Name = mongoUser.Username });
            }

            foreach (Region region in _mongoRepository.All<Region>())
            {
                ViewBag.Regions.Add(new BaseProjectController.NestedRegionViewModel() { RegionName = region.RegionName });
            }
            return View(_repository.GetGreenFieldProject());
        }

        public ActionResult MoreInfo(string id)
        {
            GreenField model = _repository.GetProjectByID<GreenField>(id);

            return View(model);
        }


        public ActionResult InsertGreenField()
        {
            return View();
        }


        #endregion

        #region Grid Actions

        [GridAction]
        public ActionResult _SelectAjaxEditing()
        {
            return View(new GridModel(_repository.GetGreenFieldProject()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _SaveAjaxEditing(string id)
        {
            if (_repository.GetProjectByID<GreenField>(id) != null)
            {
                GreenField gr = _repository.GetProjectByID<GreenField>(id);
                UpdateModel(gr);
                _repository.UpdateOne(gr);
            }

            return View(new GridModel(_repository.GetGreenFieldProject()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteAjaxEditing(string id)
        {
            if (_repository.GetProjectByID<GreenField>(id) != null)
            {
                GreenField gr = _repository.GetProjectByID<GreenField>(id);
                UpdateModel(gr);
                _repository.Delete(gr);
            }

            //Rebind the grid
            return View(new GridModel(_repository.GetGreenFieldProject()));
        }

        #endregion

        #region Helper Model Class

        public class NestedUserViewModel
        {
            public string Name { get; set; }
        }

        public class NestedRegionViewModel
        {
            public string RegionName { get; set; }
        }

        #endregion
    }
}
