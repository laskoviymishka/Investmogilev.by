using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common.Model.ProjectModels;
using MongoRepository;

namespace InvestPortal.Controllers
{
    public class WorkflowActionsController : Controller
    {
        private readonly ProjectStateManager _stateManager;

        public WorkflowActionsController()
        {
            _stateManager = new ProjectStateManager();
        }

        public ActionResult CompletePlan(string id)
        {
            ViewBag.ProjectId = id;
            return PartialView(RepositoryContext.Current.GetOne<Project>(p => p._id == id));
        }


        public ActionResult BackToTasks(string id)
        {
            _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
            _stateManager.BackToTasks(id);
            return RedirectToAction("PlanForProject", "Task", new { @id = id });
        }
    }
}
