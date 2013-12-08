using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common.Model.Common;
using Invest.Common.Model.ProjectModels;
using Invest.Common.Model.User;
using Invest.Common.Repository;
using InvestPortal.Models;
using MongoDB.Bson;
using MongoRepository;
using Telerik.Web.Mvc;
using Invest.Common.State;

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

        #region Fill Project

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
                initial.Contact = model.Contact;
                initial.Region = model.Region;
                initial.Address = new Address { Lat = model.Address.Lat, Lng = model.Address.Lng };

                RepositoryContext.Current.Update(initial);
                _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
                _stateManager.FillProject(model._id);
                return RedirectToAction("WorkFlowForProject", "BaseProject", new { id = model._id });
            }

            return View(model);
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

        #region Grid Methods project workflow

        [GridAction]
        public ActionResult _SelectAjaxEditingIndex()
        {
            return View(new GridModel(AllProjectForUser));
        }

        #endregion


        #endregion

        #region All project

        public ActionResult All()
        {
            BindUsersAndRegions();
            return View(RepositoryContext.Current.All<Project>());
        }

        #region Grid Methods project workflow

        [GridAction]
        public ActionResult _SelectAjaxEditingAll()
        {
            BindUsersAndRegions();
            return View(new GridModel(RepositoryContext.Current.All<Project>()));
        }

        #endregion
        #endregion

        #region VerigyResponse

        #region Main Actions

        public ActionResult VerifyResponse()
        {
            return View(InvestorResponses);
        }

        public ActionResult ToProject(string id)
        {
            var responsedProject = RepositoryContext.Current.All<Project>(p => p.Responses != null && p.WorkflowState.CurrenState == ProjectStates.WaitForAdminInvestorApprove);
            foreach (Project project in responsedProject)
            {
                var investorResponse = project.Responses.FirstOrDefault(p => p.ResponseId == id);
                if (investorResponse != null)
                {
                    return RedirectToAction("WorkFlowForProject", "BaseProject", new { @id = project._id });
                }
            }
            return HttpNotFound();
        }

        public ActionResult ProcessVerifyResponse(string id)
        {
            var responsedProject = RepositoryContext.Current.All<Project>(p => p.Responses != null && p.WorkflowState.CurrenState == ProjectStates.WaitForAdminInvestorApprove);
            foreach (Project project in responsedProject)
            {
                var investorResponse = project.Responses.FirstOrDefault(p => p.ResponseId == id);
                if (investorResponse != null)
                {
                    if (string.IsNullOrEmpty(investorResponse.ExistingUser))
                    {
                        var model = new InvestorRegisterModel();
                        model.Email = investorResponse.InvestorEmail;
                        model.ResponseId = id;
                        model.ResponseProjectId = project._id;
                        model.Password = ObjectId.GenerateNewId().ToString().Substring(0, 5);
                        return View(model);
                    }

                    _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
                    if (_stateManager.ProcessVerifyResponse(investorResponse.ResponsedProjectId,
                        investorResponse.ResponseId,
                        User.Identity.Name,
                        investorResponse.ExistingUser))
                    {
                        return RedirectToAction("WorkFlowForProject", "BaseProject", new { @id = investorResponse.ResponsedProjectId });
                    }
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ProcessVerifyResponse(InvestorRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
                if (_stateManager.ProcessVerifyResponse(model.ResponseProjectId,
                    model.ResponseId,
                    User.Identity.Name,
                    model.UserName,
                    model.Password,
                    model.Email))
                {
                    return RedirectToAction("WorkFlowForProject", "BaseProject", new { @id = model.ResponseProjectId });
                }

                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult UserApproveCompletion(string projectId)
        {
            _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
            _stateManager.UserApproveCompletion(projectId);
            return RedirectToAction("WorkFlowForProject", "BaseProject", new { @id = projectId });
        }

        public ActionResult AdminApproveCompletion(string projectId)
        {
            _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
            _stateManager.AdminApproveCompletion(projectId);
            return RedirectToAction("WorkFlowForProject", "BaseProject", new { @id = projectId });
        }

        #endregion

        #region Grid Methods

        [GridAction]
        public ActionResult _SelectAjaxEditing()
        {
            return View(new GridModel(InvestorResponses));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteAjaxEditing(string id)
        {
            _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
            if (_stateManager.DeleteResponse(id, User.Identity.Name))
            {
                return View(new GridModel(InvestorResponses));
            }

            return HttpNotFound();
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

        #endregion

        #region Private Helpers

        private IEnumerable<Project> AllProjectForUser
        {
            get
            {
                BindUsersAndRegions();
                if (User.IsInRole("Investor"))
                {
                    return RepositoryContext.Current.All<Project>(r => r.InvestorUser == User.Identity.Name);
                }
                return RepositoryContext.Current.All<Project>(r => r.AssignUser == User.Identity.Name);
            }
        }

        private List<InvestorResponse> InvestorResponses
        {
            get
            {
                var responsedProject =
                    RepositoryContext.Current.All<Project>(
                        r => r.Responses != null && r.WorkflowState.CurrenState == ProjectStates.WaitForAdminInvestorApprove);
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
            ViewBag.Regions = new List<NestedRegionViewModel>();
            foreach (Users mongoUser in _mongoRepository.All<Users>())
            {
                ViewBag.Users.Add(new NestedUserViewModel() { Name = mongoUser.Username });
            }

            foreach (Region region in _mongoRepository.All<Region>())
            {
                ViewBag.Regions.Add(new NestedRegionViewModel() { RegionName = region.RegionName });
            }
        }

        #endregion
    }
}
