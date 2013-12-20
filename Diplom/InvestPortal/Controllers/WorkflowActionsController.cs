using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common;
using Invest.Common.Model.Project;

namespace InvestPortal.Controllers
{
    public class WorkflowActionsController : Controller
    {
        public ActionResult CompletePlan(string id)
        {
            return PartialView();
        }


        public ActionResult BackToTasks(string id)
        {
            return RedirectToAction("PlanForProject", "Task", new { @id = id });
        }
    }
}
