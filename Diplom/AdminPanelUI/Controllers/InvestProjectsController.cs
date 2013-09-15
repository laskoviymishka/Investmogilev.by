using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Model;
using Newtonsoft.Json;

namespace AdminPanelUI.Controllers
{
	public class InvestProjectsController : Controller
	{
		class LatLng 
		{
			public Nullable<double> Lat = 0;
			public Nullable<double> Lng = 0;
			public string Name = "";
			public string Description = "";
		}

		private dbRegionsEntities db = new dbRegionsEntities();

		//
		// GET: /InvestProjects/

		public ActionResult Index()
		{
			IEnumerable<Project> projects = db.Projects.Include(p => p.RegionNav).Include(p => p.InvestCategory).ToList();
			IList<LatLng> latLngs = new List<LatLng>();

			foreach (var project in projects) 
			{
				latLngs.Add(new LatLng()
				{
					Lat = project.Lat,
					Lng = project.Lng,
					Name = project.Name,
					Description = project.Description
				});
			}
			ViewBag.LatLng = JsonConvert.SerializeObject(latLngs);
			return View(projects.ToList());
		}

		//
		// GET: /InvestProjects/ForRegion

		public ActionResult ForRegion(string id)
		{
			var projects = db.Projects.Include(p => p.RegionNav).Include(p => p.InvestCategory).Where(p => p.RegionNav.Name == id);
			return View(projects.ToList());
		}

		//
		// GET: /InvestProjects/Details/5

		public ActionResult Details(int id = 0)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return HttpNotFound();
			}
			return View(project);
		}

		//
		// GET: /InvestProjects/Create

		public ActionResult Create()
		{
			ViewBag.Region = new SelectList(db.Regions, "Name", "Name");
			ViewBag.CategoryID = new SelectList(db.InvestCategories, "Name", "Name");
			return View();
		}

		//
		// POST: /InvestProjects/Create

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Project project)
		{
			if (ModelState.IsValid)
			{
				db.Projects.Add(project);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.Region = new SelectList(db.Regions, "Name", "Description", project.Region);
			ViewBag.CategoryID = new SelectList(db.InvestCategories, "Name", "Description", project.CategoryID);
			return View(project);
		}

		//
		// GET: /InvestProjects/Edit/5

		public ActionResult Edit(int id = 0)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return HttpNotFound();
			}
			ViewBag.Region = new SelectList(db.Regions, "Name", "Description", project.Region);
			ViewBag.CategoryID = new SelectList(db.InvestCategories, "Name", "Description", project.CategoryID);
			return View(project);
		}

		//
		// POST: /InvestProjects/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Project project)
		{
			if (ModelState.IsValid)
			{
				db.Entry(project).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.Region = new SelectList(db.Regions, "Name", "Description", project.Region);
			ViewBag.CategoryID = new SelectList(db.InvestCategories, "Name", "Description", project.CategoryID);
			return View(project);
		}

		//
		// GET: /InvestProjects/Delete/5

		public ActionResult Delete(int id = 0)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
			{
				return HttpNotFound();
			}
			return View(project);
		}

		//
		// POST: /InvestProjects/Delete/5

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Project project = db.Projects.Find(id);
			db.Projects.Remove(project);
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