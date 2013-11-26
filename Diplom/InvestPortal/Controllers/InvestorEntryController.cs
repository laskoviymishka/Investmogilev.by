using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invest.Common.Repository;
using Invest.Common.Model;
using Invest.Workflow.Project;
using MongoDB.Bson;
using MongoRepository;
using MongoRepository = Invest.Common.Repository.MongoRepository;
using Invest.Workflow.StateManagment;

namespace InvestPortal.Controllers
{
    public class InvestorEntryController : Controller
    {

        #region Private Field

        private readonly ProjectRepository _projectRepository;
        private readonly IRepository _responseRepository;
        private readonly IWorkflowContext _greenFieldWorkflowContext;
        private readonly Dictionary<string, object> _conditions;

        #endregion

        #region Constructor

        public InvestorEntryController()
        {
            _projectRepository = new ProjectRepository();
            _responseRepository = new Invest.Common.Repository.MongoRepository("mongodb://tserakhau.cloudapp.net", "Projects");
            _greenFieldWorkflowContext = new GreenFieldWorkflowContext(_responseRepository);
            _conditions = new Dictionary<string, object>();
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
                var project = RepositoryContext.Current.GetOne<Project>(pr => pr._id == response.ResponsedProjectId);
               
                project.WorkflowState.CurrenState = GreenFieldStates.WaitForVerifyResponse;
                if (project.WorkflowState.ChangeHistory == null)
                {
                    project.WorkflowState.ChangeHistory = new List<History>();
                }
                project.WorkflowState.ChangeHistory.Add(
                    new History()
                        {
                            EditingTime = DateTime.Now,
                            Editor = User.Identity.Name,
                            FromState = GreenFieldStates.Open,
                            ToState = GreenFieldStates.WaitForVerifyResponse
                        });
                if (project.Responses == null)
                {
                    project.Responses = new List<InvestorResponse>();
                }
                project.Responses.Add(response);

                RepositoryContext.Current.Update(project);
                return RedirectToAction("Index");
            }
            return View(response);
        }
    }
}
