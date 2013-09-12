using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Model;

namespace AdminPanelUI.Controllers
{
	public class RegionController : Controller
	{
		private dbRegionsEntities db = new dbRegionsEntities();

		//
		// GET: /Region/

		public ActionResult Index()
		{
			return View(db.Regions.ToList());
		}

		//
		// GET: /Region/Details/5

		public ActionResult Details(string id = null)
		{
			Region region = db.Regions.Find(id);
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
				db.Regions.Add(region);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(region);
		}

		//
		// GET: /Region/Edit/5

		public ActionResult Edit(string id = null)
		{
			Region region = db.Regions.Find(id);
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
				db.Entry(region).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(region);
		}

		//
		// GET: /Region/Delete/5

		public ActionResult Delete(string id = null)
		{
			Region region = db.Regions.Find(id);
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
			Region region = db.Regions.Find(id);
			db.Regions.Remove(region);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		//
		// GET: /Region/ParametrsForRegion/5

		public ActionResult ParametrsForRegion(string id)
		{
			Region region = db.Regions.Find(id);
			if (region == null)
			{
				return HttpNotFound();
			}
			ViewData["Parametrs"] = db.Parametrs;
			ViewData["Region"] = id;
			return View(region.ParametrValues);
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}