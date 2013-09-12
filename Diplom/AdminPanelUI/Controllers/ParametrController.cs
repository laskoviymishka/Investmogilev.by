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
	public class ParametrController : Controller
	{
		private dbRegionsEntities db = new dbRegionsEntities();

		//
		// GET: /Parametr/

		public ActionResult Index()
		{
			var parametrs = db.Parametrs.Include(p => p.AgregateParametr1);
			ViewData["Parametrs"] = db.AgregateParametrs;
			return View(parametrs.ToList().OrderBy(t => t.AgregateParametr1.Name));
		}

		//
		// GET: /Parametr/Details/5

		public ActionResult Details(string id = null)
		{
			Parametr parametr = db.Parametrs.Find(id);
			if (parametr == null)
			{
				return HttpNotFound();
			}
			return View(parametr);
		}

		//
		// GET: /Parametr/Create

		public ActionResult Create()
		{
			ViewBag.AgregateParametr = new SelectList(db.AgregateParametrs, "Name", "Description");
			return View();
		}

		//
		// POST: /Parametr/Create

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Parametr parametr)
		{
			if (ModelState.IsValid)
			{
				db.Parametrs.Add(parametr);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.AgregateParametr = new SelectList(db.AgregateParametrs, "Name", "Description", parametr.AgregateParametr);
			return View(parametr);
		}

		//
		// GET: /Parametr/Edit/5

		public ActionResult Edit(string id = null)
		{
			Parametr parametr = db.Parametrs.Find(id);
			if (parametr == null)
			{
				return HttpNotFound();
			}
			ViewBag.AgregateParametr = new SelectList(db.AgregateParametrs, "Name", "Description", parametr.AgregateParametr);
			return View(parametr);
		}

		//
		// POST: /Parametr/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Parametr parametr)
		{
			if (ModelState.IsValid)
			{
				db.Entry(parametr).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.AgregateParametr = new SelectList(db.AgregateParametrs, "Name", "Description", parametr.AgregateParametr);
			return View(parametr);
		}

		//
		// GET: /Parametr/Delete/5

		public ActionResult Delete(string id = null)
		{
			Parametr parametr = db.Parametrs.Find(id);
			if (parametr == null)
			{
				return HttpNotFound();
			}
			return View(parametr);
		}

		//
		// POST: /Parametr/Delete/5

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(string id)
		{
			Parametr parametr = db.Parametrs.Find(id);
			db.Parametrs.Remove(parametr);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}