using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoRepository.Repository;
using MongoRepository.Model;
using AdminPanelUI.Models;

namespace AdminPanelUI.Controllers
{
    public class RegionController : Controller
    {
        private RegionRepository db = new RegionRepository();

        //
        // GET: /Region/

        public ActionResult Index()
        {
            return View(db.AllRegion());
        }

        //
        // GET: /Region/Details/5

        public ActionResult Details(string id = null)
        {
            Region region = db.GetRegionByID(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        //
        // GET: /Region/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Region/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Region region)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(region);
        }

        //
        // GET: /Region/Edit/5

        public ActionResult Edit(string id = null)
        {
            Region region = db.GetRegionByID(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        //
        // POST: /Region/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Region region)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(region);
        }

        //
        // GET: /Region/Delete/5

        public ActionResult Delete(string id = null)
        {
            Region region = db.GetRegionByID(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        //
        // POST: /Region/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            return RedirectToAction("Index");
        }

        //
        // GET: /Region/ParametrsForRegion/5

        public ActionResult RegionParametrs(string id)
        {
            Region region = db.GetRegionByID(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return PartialView(region);
        }

        public ActionResult ChildParametr(string regionId, string parametrName)
        {
            Region region = db.GetRegionByID(regionId);
            if (region == null)
            {
                return HttpNotFound();
            }
            foreach (var parametr in region.Parametrs)
            {
                if (parametr.ChildParametrs.Where(p => p.ParametrName == parametrName).Count() > 0)
                {
                    EditParametrViewModel viewModel = new EditParametrViewModel();
                    viewModel.RegionId = regionId;
                    viewModel.ParametrName = parametrName;
                    viewModel.Values = parametr.ChildParametrs.Where(p => p.ParametrName == parametrName).First().Values;
                    return PartialView(viewModel);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChildParametr(EditParametrViewModel region)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return PartialView(region);
        }

        //
        // GET: /Region/ParametrsForRegion/5

        public ActionResult InvestProjects(string id)
        {
            Region region = db.GetRegionByID(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}