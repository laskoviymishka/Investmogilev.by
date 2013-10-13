using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MongoRepository.Repository;
using MongoRepository.Model;
using DataAccess.Model;

namespace AdminPanelUI.Controllers
{
    public class InvestProjectsController : Controller
    {
        #region Nested class
        class LatLng
        {
            public Nullable<double> Lat = 0;
            public Nullable<double> Lng = 0;
            public string Name = "";
            public string _id = "";
            public string Description = "";
            public string Type = "";
        }

        #endregion

        #region Fields

        private ProjectRepository _repository = new ProjectRepository();

        private dbRegionsEntities db = new dbRegionsEntities();

        #endregion

        #region CommonMethods

        //
        // GET: /InvestProjects/

        public ActionResult Index()
        {
            IEnumerable<Project> projects = _repository.AllProjects();
            IList<LatLng> latLngs = GenerateGeoJsonData(projects);
            ViewBag.LatLng = JsonConvert.SerializeObject(GenerateGeoJsonData(projects));
            return View(projects.ToList());
        }

        public ActionResult ByType(string id)
        {
            IEnumerable<Project> projects = _repository.AllProjects().Where(p => p.GetType().Name == id);
            IList<LatLng> latLngs = new List<LatLng>();
            foreach (var project in projects)
            {
                latLngs.Add(new LatLng()
                {
                    Lat = project.Address.Lat,
                    Lng = project.Address.Lng,
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
            var projects = _repository.AllProjects().Where(p => p.Region == id);
            return View(projects.ToList());
        }


        #endregion

        #region TypedMethods

        #region BrownField

        public ActionResult DetailsBrownField(string id)
        {
            Project project = _repository.GetProjectByID<BrownField>(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        public ActionResult CreateBrownField()
        {
            ViewBag.Region = new SelectList(GetRegionsList());
            return View();
        }


        [HttpPost]
        public ActionResult CreateBrownField(BrownField project)
        {
            if (ModelState.IsValid)
            {
                _repository.InsertProject(project);
                return RedirectToAction("Index");
            }
            ViewBag.Region = new SelectList(GetRegionsList());
            return View(project);
        }

        public ActionResult EditBrownField(string id = "")
        {
            BrownField project = _repository.GetProjectByID<BrownField>(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.Region = new SelectList(GetRegionsList());
            return View(project);
        }

        [HttpPost]
        public ActionResult EditBrownField(BrownField project)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateOne<BrownField>(project);
                return RedirectToAction("Index");
            }
            ViewBag.Region = new SelectList(GetRegionsList());
            return View(project);
        }

        public ActionResult DeleteBrownField(string id)
        {
            //ProjectDTO project = db.Projects.Find(id);
            //if (project == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedBrownField(string id)
        {
            //ProjectDTO project = db.Projects.Find(id);
            //db.Projects.Remove(project);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region GreenField

        public ActionResult DetailsGreenField(string id)
        {
            GreenField project = _repository.GetProjectByID<GreenField>(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        public ActionResult CreateGreenField()
        {
            ViewBag.Region = new SelectList(GetRegionsList());
            return View();
        }


        [HttpPost]
        public ActionResult CreateGreenField(GreenField project)
        {
            if (ModelState.IsValid)
            {
                _repository.InsertProject(project);
                return RedirectToAction("Index");
            }
            ViewBag.Region = new SelectList(GetRegionsList());
            return View(project);
        }

        public ActionResult EditGreenField(string id = "")
        {
            GreenField project = _repository.GetProjectByID<GreenField>(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.Region = new SelectList(GetRegionsList());
            return View(project);
        }

        [HttpPost]
        public ActionResult EditGreenField(GreenField project)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateOne<GreenField>(project);
                return RedirectToAction("Index");
            }
            ViewBag.Region = new SelectList(GetRegionsList());
            return View(project);
        }

        public ActionResult DeleteGreenField(string id)
        {
            //ProjectDTO project = db.Projects.Find(id);
            //if (project == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedGreenField(string id)
        {
            //ProjectDTO project = db.Projects.Find(id);
            //db.Projects.Remove(project);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region UnusedBuilding


        public ActionResult DetailsUnusedBuilding(string id)
        {
            UnUsedBuilding project = _repository.GetProjectByID<UnUsedBuilding>(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        public ActionResult CreateUnUsedBuilding()
        {
            ViewBag.Region = new SelectList(GetRegionsList());
            return View();
        }


        [HttpPost]
        public ActionResult CreateUnUsedBuilding(UnUsedBuilding project)
        {
            if (ModelState.IsValid)
            {
                _repository.InsertProject(project);
                return RedirectToAction("Index");
            }
            ViewBag.Region = new SelectList(GetRegionsList());
            return View(project);
        }

        public ActionResult EditUnUsedBuilding(string id = "")
        {
            UnUsedBuilding project = _repository.GetProjectByID<UnUsedBuilding>(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.Region = new SelectList(GetRegionsList());
            return View(project);
        }

        [HttpPost]
        public ActionResult EditUnUsedBuilding(UnUsedBuilding project)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateOne<UnUsedBuilding>(project);
                return RedirectToAction("Index");
            }
            ViewBag.Region = new SelectList(GetRegionsList());
            return View(project);
        }

        public ActionResult DeleteUnUsedBuilding(string id)
        {
            //ProjectDTO project = db.Projects.Find(id);
            //if (project == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedUnUsedBuilding(string id)
        {
            //ProjectDTO project = db.Projects.Find(id);
            //db.Projects.Remove(project);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }


        #endregion

        #endregion

        #region External method

        public ActionResult ListOfProjects()
        {
            List<Project> result = new List<Project>();
            result.AddRange(_repository.GetGreenFieldProject().Take(2));
            result.AddRange(_repository.GetBrownFieldsProject().Take(2));
            result.AddRange(_repository.GetUnUsedBuildingProject().Take(2));
            return PartialView(result);
        }

        public ActionResult PopUpDetailsGreenField(string id)
        {
            Project project = _repository.GetProjectByID<GreenField>(id);

            if (project == null)
            {
                return null;
            }
            else
            {
                return PartialView(project);
            }
        }

        public ActionResult PopUpDetailsBrownField(string id)
        {
            Project project = _repository.GetProjectByID<BrownField>(id);
            if (project == null)
            {
                return null;
            }
            else
            {
                return PartialView(project);
            }
        }

        public ActionResult PopUpDetailsUnUsedBuilding(string id)
        {
            Project project = project = _repository.GetProjectByID<UnUsedBuilding>(id);
            if (project == null)
            {
                return null;
            }
            else
            {
                return PartialView(project);
            }
        }

        public string ProjectGeoJSON()
        {
            return JsonConvert.SerializeObject(GenerateGeoJsonData(_repository.AllProjects()));
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region Private Helpers

        private IList<string> GetRegionsList()
        {
            IList<string> regions = new List<string>();
            foreach (var item in db.Regions)
            {
                regions.Add(item.Name);
            }
            return regions;
        }

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

        #endregion
    }
}