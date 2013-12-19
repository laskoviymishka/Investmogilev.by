using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common;
using Invest.Common.Model.ProjectWorkflow;
using Invest.Common.Repository;
using MongoDB.Bson;
using InvestPortal.Models;

namespace InvestPortal.Controllers
{
    public class InvestorEntryController : Controller
    {
        #region Private Field

        private readonly IRepository _responseRepository;
        private readonly Dictionary<string, object> _conditions;
        private readonly ProjectStateManager _stateManager;

        #endregion

        #region Constructor

        public InvestorEntryController()
        {
            _projectRepository = new ProjectRepository();
            _responseRepository = new Invest.Common.Repository.MongoRepository("mongodb://tserakhau.cloudapp.net", "Projects");
            _greenFieldWorkflowContext = new GreenFieldWorkflowContext(_responseRepository);
            _conditions = new Dictionary<string, object>();
            _stateManager = new ProjectStateManager();
        }

        #endregion

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResponseToProject(string id)
        {
            Project project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == id);
            if (project != null)
            {
                ViewBag.ProjectId = id;
                return View();
            }
            else
            {
                return HttpNotFound("Проект не найден, свяжитесь с администратором");
            }
        }

        [AllowAnonymous]
        public ActionResult NewResponseToProject(string id)
        {
            Project project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == id);
            if (project != null)
            {
                InvestorResponse responseViewModel = new InvestorResponse();
                responseViewModel.ResponsedProjectId = id;
                responseViewModel.ResponseId = ObjectId.GenerateNewId().ToString();
                responseViewModel.ResponseDate = DateTime.Now;
                return PartialView(responseViewModel);
            }
            else
            {
                return HttpNotFound("Проект не найден, свяжитесь с администратором");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult NewResponseToProject(InvestorResponse model)
        {
            if (ModelState.IsValid)
            {
                _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
                _stateManager.ResponseToProject(model, User.Identity.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }


        [AllowAnonymous]
        public ActionResult ExistingResponseToProject(string id)
        {
            Project project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == id);
            if (project != null)
            {
                ExistingResponseViewModel viewModel = new ExistingResponseViewModel();
                viewModel.ProjectId = id;
                viewModel.ResponseId = ObjectId.GenerateNewId().ToString();
                viewModel.ResponseTime = DateTime.Now;
                return PartialView(viewModel);
            }
            else
            {
                return HttpNotFound("Проект не найден, свяжитесь с администратором");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ExistingResponseToProject(ExistingResponseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = RepositoryContext.Current.GetOne<Project>(p => p.InvestorUser == model.UserName);
                if (project != null)
                {
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("ExistingUser", new AuthenticationException("пользователь не существует"));
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [Authorize(Roles = "Investor")]
        public ActionResult ApproveResponse(string id)
        {
            _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
            _stateManager.ApproveResponseByInvestor(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Investor")]
        public ActionResult ApprovePlan(string id)
        {
            _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
            _stateManager.ApprovePlanByInvestor(id);
            return RedirectToAction("Index");
        }
    }
}
