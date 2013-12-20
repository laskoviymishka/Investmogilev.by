using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using MongoDB.Bson;
using InvestPortal.Models;

namespace InvestPortal.Controllers
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
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Investor")]
        public ActionResult ApprovePlan(string id)
        {
            return RedirectToAction("Index");
        }
    }
}
