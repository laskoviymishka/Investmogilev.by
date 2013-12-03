using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common.Model.ProjectModels;
using Invest.Common.Repository;
using Invest.Workflow.Project;
using Invest.Workflow.StateManagment;
using MongoDB.Bson;
using MongoRepository;

namespace InvestPortal.Controllers
{
    public class InvestorEntryController : Controller
    {
        #region Private Field

        private readonly ProjectRepository _projectRepository;
        private readonly IRepository _responseRepository;
        private readonly IWorkflowContext _greenFieldWorkflowContext;
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
                InvestorResponse response = new InvestorResponse();
                response.ResponsedProjectId = id;
                response.ResponseId = ObjectId.GenerateNewId().ToString();
                response.ResponseDate = DateTime.Now;
                return View(response);
            }
            else
            {
                return HttpNotFound("Проект не найден, свяжитесь с администратором");
            }
        }

        [HttpPost]
        public ActionResult ResponseToProject(InvestorResponse response)
        {
            if (ModelState.IsValid)
            {
                _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
                _stateManager.ResponseToProject(response, User.Identity.Name);

                return RedirectToAction("Index");
            }
            return View(response);
        }

        [Authorize(Roles = "Investor")]
        public ActionResult ApproveResponse(string id)
        {
            _stateManager.SetContext(User.Identity.Name,Roles.GetRolesForUser(User.Identity.Name));
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
