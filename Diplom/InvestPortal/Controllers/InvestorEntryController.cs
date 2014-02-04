using System;
using System.Security.Authentication;
using System.Web.Mvc;
using Investmogilev.Infrastructure.BusinessLogic.Managers;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.UI.Portal.Models;
using MongoDB.Bson;

namespace Investmogilev.UI.Portal.Controllers
{
    public class InvestorEntryController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResponseToProject(string id)
        {
            var project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == id);
            if (project != null)
            {
                ViewBag.ProjectId = id;
                return View();
            }
            return HttpNotFound("Проект не найден, свяжитесь с администратором");
        }

        [AllowAnonymous]
        public ActionResult NewResponseToProject(string id)
        {
            var project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == id);
            if (project != null)
            {
                var responseViewModel = new InvestorResponse();
                responseViewModel.ProjectId = id;
                responseViewModel.ResponseId = ObjectId.GenerateNewId().ToString();
                responseViewModel.ResponseDate = DateTime.Now;
                return PartialView(responseViewModel);
            }
            return HttpNotFound("Проект не найден, свяжитесь с администратором");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult NewResponseToProject(InvestorResponse model)
        {
            if (ModelState.IsValid)
            {
                ProjectStateManager.StateManagerFactory(model.ProjectId, null, null).ResponsedOnProject(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult ExistingResponseToProject(string id)
        {
            var project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == id);
            if (project != null)
            {
                var viewModel = new ExistingResponseViewModel();
                viewModel.ProjectId = id;
                viewModel.ResponseId = ObjectId.GenerateNewId().ToString();
                viewModel.ResponseTime = DateTime.Now;
                return PartialView(viewModel);
            }
            return HttpNotFound("Проект не найден, свяжитесь с администратором");
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

                ModelState.AddModelError("ExistingUser", new AuthenticationException("пользователь не существует"));
                return View(model);
            }

            return View(model);
        }

        [Authorize(Roles = "Investor")]
        public ActionResult ApprovePlan(string id)
        {
            return RedirectToAction("Index");
        }
    }
}