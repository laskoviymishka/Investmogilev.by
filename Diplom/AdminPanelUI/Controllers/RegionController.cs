using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invest.Common.Model;
using MongoRepository.Repository;
using AdminPanelUI.Models;

namespace AdminPanelUI.Controllers
{
    [Authorize]
    public class RegionController : Controller
    {
        private RegionRepository db = new RegionRepository();

        //
        // GET: /Region/

        public ActionResult Index()
        {
            ViewBag.StartYear = 2005;
            ViewBag.EndYear = 2012;
            return View(db.GetAll());
        }

        public ActionResult ChildParametr(string regionId, int parametrName)
        {
            Region region = db.GetById(regionId);
            if (region == null)
            {
                return HttpNotFound();
            }
            foreach (var parametr in region.Parametrs)
            {
                foreach (var item in parametr.ChildParametrs)
                {
                    if (item.ParametrName.GetHashCode() == parametrName)
                    {
                        EditParametrViewModel viewModel = new EditParametrViewModel();
                        viewModel.RegionId = regionId;
                        viewModel.ParametrName = item.ParametrName;
                        viewModel.Values = parametr.ChildParametrs.Where(p => p.ParametrName == item.ParametrName).First().Values;
                        return PartialView(viewModel);
                    }
                }
            }
            return HttpNotFound();
        }

        public ActionResult RegionParametr(string id)
        {
            ViewBag.StartYear = 2005;
            ViewBag.EndYear = 2012;
            Region region = db.GetById(id);
            return View(region);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public object ChildParametr(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                Region region = db.GetById(collection["RegionId"]);
                foreach (var parametr in region.Parametrs)
                {
                    foreach (var item in parametr.ChildParametrs)
                    {
                        if (item.ParametrName.GetHashCode().ToString() == collection["ParametrName"])
                        {
                            item.Values.Clear();
                            for (int i = 0; i < (collection.Count - 3) / 2; i++)
                            {
                                double res;
                                int year;
                                if (collection[string.Format("[{0}].Value", i)] != null &&
                                    double.TryParse(collection[string.Format("[{0}].Value", i)], out res) &&
                                    collection[string.Format("[{0}].Key", i)] != null &&
                                    int.TryParse(collection[string.Format("[{0}].Key", i)], out year))
                                {
                                    item.Values.Add(new KeyValuePair<int, double>(year, res));
                                }
                            }
                        }
                    }
                }
                db.Update(region);
            }
            return RedirectToAction("index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}