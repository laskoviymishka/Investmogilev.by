using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invest.Common.Repository;
using Invest.Common.Model;
using MongoRepository = Invest.Common.Repository.MongoRepository;

namespace InvestPortal.Controllers
{
    public class InvestorEntryController : Controller
    {

        #region Private Field

        private readonly ProjectRepository _projectRepository;
        private readonly IRepository _responseRepository;

        #endregion

        #region Constructor

        public InvestorEntryController()
        {
            _projectRepository = new ProjectRepository();
            _responseRepository = new Invest.Common.Repository.MongoRepository("mongodb://tserakhau.cloudapp.net", "Projects");
        }

        #endregion

        //
        // GET: /InvestorEntry/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResponseToProject(string id)
        {
            Project project = null;

            if (_projectRepository.GetProjectByID<BrownField>(id) != null)
            {
                project = _projectRepository.GetProjectByID<BrownField>(id);
            }

            if (_projectRepository.GetProjectByID<UnUsedBuilding>(id) != null)
            {
                project = _projectRepository.GetProjectByID<UnUsedBuilding>(id);
            }

            if (_projectRepository.GetProjectByID<GreenField>(id) != null)
            {
                project = _projectRepository.GetProjectByID<GreenField>(id);
            }

            if (project != null)
            {
                InvestorResponse response = new InvestorResponse();
                response.ResponsedProjectId = id;
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
                _responseRepository.Add(response);
                return RedirectToAction("Index");
            }
            return View(response);
        }
    }
}
