using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common;
using Invest.Common.Model.Project;
using Invest.Common.Model.User;
using Invest.Common.Repository;

namespace InvestPortal.Controllers
{
    public class BaseProjectController : Controller
    {
        #region Private Fields

        private readonly IRepository _mongoRepository;
        private readonly ProjectStateManager _stateManager;

        #endregion

        #region Constructor

        public BaseProjectController()
        {
            _mongoRepository = RepositoryContext.Current;
            _stateManager = new ProjectStateManager();
        }

        #endregion

        #region Create Project


        public ActionResult CreateGreenFieldProject()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateGreenFieldProject(GreenField model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Project", "BaseProject", new { id = model._id });
            }

            return View(model);
        }

        public ActionResult CreateUnUsedBuildingProject()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateUnUsedBuildingProject(UnUsedBuilding model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Project", "BaseProject", new { id = model._id });
            }

            return View(model);
        }

        public ActionResult CreateProject(string id)
        {
            return View();
        }

        #endregion

        #region Fill Project

        public ActionResult GreenFieldProject(string id)
        {
            ViewBag.Tags = new List<string>() { "test", "test2", "test3" };
            return PartialView(RepositoryContext.Current.GetOne<Project>(p => p._id == id) as GreenField);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GreenFieldProject(GreenField model)
        {
            if (ModelState.IsValid)
            {
                var initial = RepositoryContext.Current.GetOne<Project>(t => t._id == model._id);
                initial.Name = model.Name;
                initial.Description = model.Description;
                initial.AddressName = model.AddressName;
                initial.Region = model.Region;
                initial.Address = new Address { Lat = model.Address.Lat, Lng = model.Address.Lng };
                initial.Tags = model.Tags;

                return RedirectToAction("Project", "BaseProject", new { id = model._id });
            }

            return View(model);
        }

        public ActionResult UnUsedBuildingProject(string id)
        {
            return PartialView(RepositoryContext.Current.GetOne<Project>(p => p._id == id) as UnUsedBuilding);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UnUsedBuildingProject(UnUsedBuilding model)
        {
            if (ModelState.IsValid)
            {
                var initial = RepositoryContext.Current.GetOne<Project>(t => t._id == model._id) as UnUsedBuilding;
                initial.Name = model.Name;
                initial.Description = model.Description;
                initial.AddressName = model.AddressName;
                initial.Region = model.Region;
                initial.Address = new Address { Lat = model.Address.Lat, Lng = model.Address.Lng };
                initial.Area = model.Area;
                initial.BalancePrice = model.BalancePrice;
                initial.IsCommunicate = model.IsCommunicate;
                initial.IsSell = model.IsSell;

                return RedirectToAction("Project", "BaseProject", new { id = model._id });
            }

            return View(model);
        }

        public ActionResult FillProject(string id)
        {
            return View(RepositoryContext.Current.GetOne<Project>(p => p._id == id));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FillProject(Project model)
        {
            if (ModelState.IsValid)
            {
                var initial = RepositoryContext.Current.GetOne<Project>(t => t._id == model._id);
                initial.Name = model.Name;
                initial.Description = model.Description;
                initial.AddressName = model.AddressName;
                initial.Region = model.Region;
                initial.Address = new Address { Lat = model.Address.Lat, Lng = model.Address.Lng };

                return RedirectToAction("Project", "BaseProject", new { id = model._id });
            }

            return View(model);
        }

        public ActionResult Project(string id)
        {
            return View(RepositoryContext.Current.GetOne<Project>(p => p._id == id));
        }

        #endregion

        #region Base project workflow for user

        public ActionResult Index()
        {
            return View(AllProjectForUser);
        }

        public ActionResult WorkFlowForProject(string id)
        {
            return View(RepositoryContext.Current.GetOne<Project>(pr => pr._id == id));
        }

        public ActionResult WorkFlowForProjectPartial(string id)
        {
            return PartialView(RepositoryContext.Current.GetOne<Project>(pr => pr._id == id));
        }

        public ActionResult ProjectInfo(string id)
        {
            return View(RepositoryContext.Current.GetOne<Project>(pr => pr._id == id));
        }

        #endregion


        #region All project

        public ActionResult All()
        {
            BindUsersAndRegions();
            return View(RepositoryContext.Current.All<Project>());
        }

        #endregion

        #region Main Actions

        public ActionResult VerifyResponse()
        {
            return View(InvestorResponses);
        }

        public ActionResult ToProject(string id)
        {
            var responsedProject = RepositoryContext.Current.All<Project>(p => p.Responses != null);
            foreach (Project project in responsedProject)
            {
                var investorResponse = project.Responses.FirstOrDefault(p => p.ResponseId == id);
                if (investorResponse != null)
                {
                    return RedirectToAction("Project", "BaseProject", new { @id = project._id });
                }
            }
            return HttpNotFound();
        }


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

        #endregion

        #region Private Helpers

        private IEnumerable<Project> AllProjectForUser
        {
            get
            {
                BindUsersAndRegions();
                if (User.IsInRole("Investor"))
                {
                    return RepositoryContext.Current.All<Project>();
                }
                return RepositoryContext.Current.All<Project>();
            }
        }

        private List<InvestorResponse> InvestorResponses
        {
            get
            {
                var responsedProject =
                    RepositoryContext.Current.All<Project>();
                var model = new List<InvestorResponse>();
                foreach (var project in responsedProject)
                {
                    model.AddRange(project.Responses);
                }
                return model;
            }
        }

        private void BindUsersAndRegions()
        {
            ViewBag.Users = new List<NestedUserViewModel>();
            foreach (Users mongoUser in _mongoRepository.All<Users>())
            {
                ViewBag.Users.Add(new NestedUserViewModel() { Name = mongoUser.Username });
            }
        }

        #endregion
    }
}
