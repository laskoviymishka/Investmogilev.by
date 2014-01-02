using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using BusinessLogic.Wokflow;
using Invest.Common;
using Invest.Common.Model.Project;
using Invest.Common.Notification;
using Invest.Common.Repository;
using Invest.Common.State;
using BusinessLogic.Notification;
using MongoDB.Bson;

namespace BusinessLogic.Managers
{
    public class ProjectStateManager
    {
        #region Constatnt

        private const string ADMIN_ROLE = "Admin";
        private const string INVESTOR_ROLE = "Investor";
        private const string RAYON_ROLE = "User";

        #endregion

        #region Private Fields

        private readonly ProjectWorkflowWrapper _workflow;
        private Project _currentProject;
        private string _currentUser;
        private IList<string> _roles;
        private readonly IInvestorNotification _investorNotificate;
        private readonly IAdminNotification _adminNotificate;
        private readonly IUserNotification _userNotificationl;
        private readonly IUnitsOfWorkContainer _unitsOfWork;
        private readonly IRepository _repository;

        #endregion

        #region Constructor

        private ProjectStateManager(Project currentProject, string currentUser, IList<string> roles)
        {
            _currentUser = currentUser;
            _roles = roles;
            _investorNotificate = new InvestorNotification();
            _adminNotificate = new AdminNotification();
            _userNotificationl = new UserNotification();
            _repository = RepositoryContext.Current;
            _currentProject = _repository.GetOne<Project>(p => p._id == currentProject._id);
            _unitsOfWork = new UnitsOfWorkContainer(_currentProject,
                _repository,
                _userNotificationl,
                _adminNotificate,
                _investorNotificate,
                _currentUser, _roles);
            if (string.IsNullOrEmpty(_currentProject._id))
            {
                _currentProject._id = ObjectId.GenerateNewId().ToString();
            }

            if (_currentProject.WorkflowState == null)
            {
                _currentProject.WorkflowState = new Workflow()
                    {
                        CurrentState = ProjectWorkflow.State.Open
                    };
            }
            _workflow = new ProjectWorkflowWrapper(new ProjectWorkflow(_currentProject.WorkflowState.CurrentState), _unitsOfWork);
        }

        #endregion

        #region Public methods

        public void CreateProject(Project createdProject)
        {
            if (_repository.GetOne<Project>(p => p._id == createdProject._id) == null)
            {
                _repository.Add(createdProject);
            }
            else
            {
                throw new ArgumentException("Project with same id currently exist");
            }
        }

        public void FillInformation(Project filledProject)
        {
            if (_workflow.IsMoveablde(ProjectWorkflow.Trigger.FillInformation))
            {
                _repository.Update(filledProject);
                _workflow.Move(ProjectWorkflow.Trigger.FillInformation);
            }
        }

        public void ResponsedOnProject(InvestorResponse response)
        {
            if (_currentProject == null)
            {
                throw new ArgumentNullException("Проект не найден");
            }

            if (_workflow.IsMoveablde(ProjectWorkflow.Trigger.InvestorResponsed))
            {
                if (_currentProject.Responses == null
                || !_currentProject.Responses.Any()
                || _currentProject.WorkflowState.CurrentState == ProjectWorkflow.State.OnMap)
                {
                    _currentProject.Responses = new List<InvestorResponse>();
                }

                _currentProject.Responses.Add(response);
                _repository.Update(_currentProject);

                if (!_workflow.Move(ProjectWorkflow.Trigger.InvestorResponsed))
                {
                    throw new InvalidOperationException("Не могу осуществить операцию InvestorResponsed");
                }

            }
            else
            {
                throw new InvalidOperationException("Не могу осуществить операцию InvestorResponsed");
            }
        }

        #endregion

        #region Factory method

        public static ProjectStateManager StateManagerFactory(Project currentProject, string currentUser, IList<string> roles)
        {
            return new ProjectStateManager(currentProject, currentUser, roles);
        }

        public static ProjectStateManager StateManagerFactory(string projectId, string currentUser, IList<string> roles)
        {
            return StateManagerFactory(RepositoryContext.Current.GetOne<Project>(p => p._id == projectId),currentUser,roles);
        }

        #endregion

        public IEnumerable<ProjectWorkflow.Trigger> GetAvaibleTriggers()
        {
            return Enum.GetValues(typeof (ProjectWorkflow.Trigger)).Cast<ProjectWorkflow.Trigger>().Where(trigger => _workflow.IsMoveablde(trigger));
        }

        public void InvestorSelected()
        {
            if(!_workflow.Move(ProjectWorkflow.Trigger.InvestorSelected))
            {
                throw new InvalidOperationException("не могу провести операцию выбора инвестора");
            }
        }
    }
}