using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invest.Common.Model;
using Invest.Common.Model.Common;
using Invest.Common.Model.ProjectModels;
using Invest.Common.Model.User;
using Invest.Common.Repository;
using InvestPortal.Models;
using MongoDB.Driver.Builders;
using MongoRepository;
using Telerik.Web.Mvc;

namespace InvestPortal.Controllers
{
    public class GreenFieldController : Controller
    {
        #region Constructor

        public GreenFieldController()
        {
        }

        #endregion

        #region Main Methods

        public ActionResult Index()
        {
            ViewBag.Users = new List<BaseProjectController.NestedUserViewModel>();
            
            foreach (Users mongoUser in RepositoryContext.Current.All<Users>())
            {
                ViewBag.Users.Add(new BaseProjectController.NestedUserViewModel() { Name = mongoUser.Username });
            }

            return View(GreenFields);
        }

        private List<GreenField> GreenFields
        {
            get
            {
                List<GreenField> model = new List<GreenField>();
                var project = RepositoryContext.Current.All<Project>().ToArray();
                foreach (Project greenField in project)
                {
                    if (greenField is GreenField)
                    {
                        model.Add(greenField as GreenField);
                    }
                }
                return model;
            }
        }

        public ActionResult MoreInfo(string id)
        {
            GreenField model = RepositoryContext.Current.GetOne<Project>(p => p._id == id) as GreenField;
            if (model == null)
            {
                return HttpNotFound();
            }

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
            return View(new GridModel(GreenFields));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _SaveAjaxEditing(string id)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == id);
            if (project != null)
            {
                UpdateModel(project);
                RepositoryContext.Current.Update(project);
            }

            return View(new GridModel(GreenFields));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteAjaxEditing(string id)
        {
            if (RepositoryContext.Current.GetOne<Project>(p => p._id == id) != null)
            {
                RepositoryContext.Current.Delete<Project>(p => p._id == id);
            }
            //Rebind the grid
            return View(new GridModel(GreenFields));
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
