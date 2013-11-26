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

        #region Base project workflow for user

        public ActionResult Index()
        {
            return View(AllProjectForUser);
        }

        public ActionResult WorkFlowForProject(string id)
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
            var responsedProject = RepositoryContext.Current.All<Project>(p => p.Responses != null && p.WorkflowState.CurrenState == GreenFieldStates.WaitForVerifyResponse);
            foreach (Project project in responsedProject)
            {
                var investorResponse = project.Responses.FirstOrDefault(p => p.ResponseId == id);
                if (investorResponse != null)
                {
                    return RedirectToAction("WorkFlowForProject", "BaseProject", new {@id = project._id});
                }
            }
            return HttpNotFound();
        }

        public ActionResult ProcessVerifyResponse(string id)
        {
            var responsedProject = RepositoryContext.Current.All<Project>(p => p.Responses != null && p.WorkflowState.CurrenState == GreenFieldStates.WaitForVerifyResponse);
            foreach (Project project in responsedProject)
            {
                var investorResponse = project.Responses.FirstOrDefault(p => p.ResponseId == id);
                if (investorResponse != null && investorResponse.IsVerified == false)
                {
                    var model = new InvestorRegisterModel();
                    model.Email = investorResponse.InvestorEmail;
                    model.ResponseId = id;
                    model.ResponseProjectId = project._id;
                    model.Password = ObjectId.GenerateNewId().ToString().Substring(0, 5);
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ProcessVerifyResponse(InvestorRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var project = RepositoryContext.Current.GetOne<Project>(r => r._id == model.ResponseProjectId);
                if (project == null)
                {
                    return HttpNotFound();
                }
                var response = project.Responses.FirstOrDefault(r => r.ResponseId == model.ResponseId);

                if (response != null) response.IsVerified = true;

                project.Responses = new List<InvestorResponse>() { response };

                string fromState = project.WorkflowState.CurrenState;
                var workflow = project.WorkflowState;
                workflow.CurrenState = GreenFieldStates.VerifyResponse;
                project.WorkflowState.ChangeHistory.Add(
                    new History()
                        {
                            EditingTime = DateTime.Now,
                            Editor = User.Identity.Name,
                            FromState = fromState,
                            ToState = GreenFieldStates.VerifyResponse
                        });
                project.InvestorUser = model.UserName;
                Membership.CreateUser(model.UserName, model.Password, model.Email);
                Roles.AddUserToRole(model.UserName, "Investor");
                RepositoryContext.Current.Update(project);

                return RedirectToAction("VerifyResponse");
            }
            return View(model);
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
            var investorResponse = InvestorResponses.FirstOrDefault(p => p.ResponseId == id);
            if (investorResponse != null)
            {
                var projectId = investorResponse.ResponsedProjectId;

                var project = RepositoryContext.Current.GetOne<Project>(r => r._id == projectId);
                if (project == null)
                {
                    return HttpNotFound();
                }
                string fromState = project.WorkflowState.CurrenState;
                var workflow = project.WorkflowState;
                project.Responses.Remove(investorResponse);
                workflow.CurrenState = GreenFieldStates.Open;
                project.WorkflowState.ChangeHistory.Add(
                    new History()
                        {
                            EditingTime = DateTime.Now,
                            Editor = User.Identity.Name,
                            FromState = fromState,
                            ToState = GreenFieldStates.Open
                        });

                RepositoryContext.Current.Update(project);
            }
            return View(new GridModel(InvestorResponses));
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

                var responsedProject =
                    RepositoryContext.Current.All<Project>(r => r.AssignUser == User.Identity.Name);
                return responsedProject.ToList();
            }
        }

        private List<InvestorResponse> InvestorResponses
        {
            get
            {
                var responsedProject =
                    RepositoryContext.Current.All<Project>(
                        r => r.Responses != null && r.WorkflowState.CurrenState == GreenFieldStates.WaitForVerifyResponse);
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
