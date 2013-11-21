using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Invest.Common.Model;
using Invest.Common.Repository;
using Invest.Workflow.Project;
using InvestPortal.Models;
using MongoDB.Bson;
using MongoRepository;
using Telerik.Web.Mvc;

namespace InvestPortal.Controllers
{
    public class BaseProjectController : Controller
    {
        #region Private Fields

        private readonly ProjectRepository _repository;
        private readonly IRepository _mongoRepository;

        #endregion

        #region Constructor

        public BaseProjectController()
        {
            _repository = new ProjectRepository();
            _mongoRepository = RepositoryContext.Current;
        }

        #endregion

        public ActionResult Index()
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
            return View(_repository.AllProjects());
        }


        public ActionResult VerifyResponse()
        {
            var responsedProject = _repository.AllProjects().Where(p => p.WorkflowState.CurrenState == GreenFieldStates.WaitForVerifyResponse).ToList();
            var model = new List<InvestorResponse>();
            foreach (var project in responsedProject)
            {
                model.AddRange(RepositoryContext.Current.All<InvestorResponse>(r => project != null && r.ResponsedProjectId == project._id));
            }

            return View(model);
        }

        public ActionResult ProcessVerifyResponse(string id)
        {
            var response = RepositoryContext.Current.GetOne<InvestorResponse>(r => r._id == id);
            InvestorRegisterModel model = new InvestorRegisterModel();
            model.Email = response.InvestorEmail;
            model.Password = ObjectId.GenerateNewId().ToString();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProcessVerifyResponse(InvestorRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Membership.CreateUser(model.UserName, model.Password, model.Password);
                var response = RepositoryContext.Current.GetOne<InvestorResponse>(r => r._id == model.ResponseId);
                response.IsVerified = true;
                Project project = _repository.GetProjectByID<GreenField>(response.ResponsedProjectId);
                if (project == null)
                {
                    project = _repository.GetProjectByID<BrownField>(response.ResponsedProjectId);
                    if (project == null)
                    {
                        project = _repository.GetProjectByID<UnUsedBuilding>(response.ResponsedProjectId);
                    }
                }
                string fromState = project.WorkflowState.CurrenState;
                project.WorkflowState.CurrenState = GreenFieldStates.VerifyResponse;
                project.WorkflowState.ChangeHistory.Add(
                    new History()
                        {
                            EditingTime = DateTime.Now,
                            Editor = User.Identity.Name,
                            FromState = fromState,
                            ToState = GreenFieldStates.VerifyResponse
                        });
                RepositoryContext.Current.Update(project.WorkflowState);

                RepositoryContext.Current.Update(response);
                RedirectToAction("VerifyResponse");
            }
            return View(model);
        }

        #region Grid Methods

        [GridAction]
        public ActionResult _SelectAjaxEditing()
        {
            return View(new GridModel(_repository.AllProjects()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _SaveAjaxEditing(string id)
        {
            if (_repository.GetProjectByID<GreenField>(id) == null)
            {
                if (_repository.GetProjectByID<BrownField>(id) == null)
                {
                    if (_repository.GetProjectByID<UnUsedBuilding>(id) == null)
                    {
                        UnUsedBuilding un = _repository.GetProjectByID<UnUsedBuilding>(id);
                        UpdateModel(un);
                        _repository.UpdateOne(un);
                    }
                }
                else
                {
                    BrownField br = _repository.GetProjectByID<BrownField>(id);
                    UpdateModel(br);
                    _repository.UpdateOne(br);
                }
            }
            else
            {
                GreenField gr = _repository.GetProjectByID<GreenField>(id);
                UpdateModel(gr);
                _repository.UpdateOne(gr);
            }

            return View(new GridModel(_repository.AllProjects()));
        }

        private void UpdateUserModel(UserManagerViewModel model, Users product)
        {
            UpdateModel(model);
            //TryUpdateModel(product);
            product.Username = model.Username;
            product.Email = model.LoweredEmail;
            product.CreationDate = model.CreationDate;
            product.IsLockedOut = model.IsLockedOut;
            product.PasswordQuestion = model.PasswordQuestion;
            product.PasswordAnswer = model.PasswordAnswer;
            product.IsApproved = model.IsApproved;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteAjaxEditing(string id)
        {
            if (_repository.GetProjectByID<GreenField>(id) == null)
            {
                if (_repository.GetProjectByID<BrownField>(id) == null)
                {
                    if (_repository.GetProjectByID<UnUsedBuilding>(id) != null)
                    {
                        UnUsedBuilding un = _repository.GetProjectByID<UnUsedBuilding>(id);
                        UpdateModel(un);
                        _repository.Delete(un);
                    }
                }
                else
                {
                    BrownField br = _repository.GetProjectByID<BrownField>(id);
                    UpdateModel(br);
                    _repository.Delete(br);
                }
            }
            else
            {
                GreenField gr = _repository.GetProjectByID<GreenField>(id);
                UpdateModel(gr);
                _repository.Delete(gr);
            }

            //Rebind the grid
            return View(new GridModel(_repository.AllProjects()));
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
