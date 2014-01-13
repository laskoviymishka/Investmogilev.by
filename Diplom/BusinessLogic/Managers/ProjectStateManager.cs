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
using Invest.Common.State.StateAttributes;
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
            if (_currentProject == null)
            {
                _currentProject = currentProject;
            }
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
            //_workflow = new ProjectWorkflowWrapper(new ProjectWorkflow(_currentProject.WorkflowState.CurrentState), _unitsOfWork);
            ProjectStateContext context = new ProjectStateContext();
            context.UserName = currentUser;
            context.CurrentProject = currentProject;
            context.Roles = roles;
            context.InvestorNotification = _investorNotificate;
            context.UserNotification = _userNotificationl;
            context.AdminNotification = _adminNotificate;
            var builder = new AttributeStateMachineBuilder();
            _workflow = new ProjectWorkflowWrapper(
                new ProjectWorkflow(
                    builder.BuilStateMachine<ProjectWorkflow.State,ProjectWorkflow.Trigger>("test", context,_currentProject.WorkflowState.CurrentState)));
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

        public void DocumentUpdate()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.DocumentUpdate))
            {
                throw new InvalidOperationException("не могу провести операцию обновление статуса отправки документов");
            }
        }

        public void FillInvolvedOrganization()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.FillInvolvedOrganization))
            {
                throw new InvalidOperationException("не могу провести операцию заполнения причастных лиц");
            }
        }

        public void InvolvedOrganizationUpdate()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.InvolvedOrganizationUpdate))
            {
                throw new InvalidOperationException("не могу провести операцию обновления статуса причастных лиц");
            }
        }

        public void ToComission()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.ToComission))
            {
                throw new InvalidOperationException("не могу провести операцию перевода в стадию на комиссии");
            }
        }

        public void Comission()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.Comission))
            {
                throw new InvalidOperationException("не могу провести операцию перевода в стадию на комиссии");
            }
        }

        public void ComissionFix()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.ComissionFix))
            {
                throw new InvalidOperationException("не могу провести операцию перевода в стадию на исправления после комиссии");
            }
        }

        public void ComissionFixUpdate()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.ComissionFixUpdate))
            {
                throw new InvalidOperationException("не могу провести операцию перевода в стадию на исправления после комиссии");
            }
        }

        public void Ispolcom()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.Ispolcom))
            {
                throw new InvalidOperationException("не могу провести операцию перевода в стадию на исправления после комиссии");
            }
        }

        public void ToIspolcom()
        {
            throw new NotImplementedException();
        }

        public void ToIspolcomFix()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.ToIspolcomFix))
            {
                throw new InvalidOperationException("не могу провести операцию ToIspolcomFix");
            }
        }

        public void IspolcomFixUpdate()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.IspolcomFixUpdate))
            {
                throw new InvalidOperationException("не могу провести операцию IspolcomFixUpdate");
            }
        }

        public void ToMinEconomy()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.ToMinEconomy))
            {
                throw new InvalidOperationException("не могу провести операцию ToMinEconomy");
            }
        }

        public void MinEconomyResponsed()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.MinEconomyResponsed))
            {
                throw new InvalidOperationException("не могу провести операцию MinEconomyResponsed");
            }
        }

        public void UpdatePlan()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.UpdatePlan))
            {
                throw new InvalidOperationException("не могу провести операцию UpdatePlan");
            }
        }

        public void ApprovePlan()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.ApprovePlan))
            {
                throw new InvalidOperationException("не могу провести операцию ApprovePlan");
            }
        }

        public void UpdateRealization()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.UpdateRealization))
            {
                throw new InvalidOperationException("не могу провести операцию UpdateRealization");
            }
        }

        public void RejectDocument()
        {
            if (!_workflow.Move(ProjectWorkflow.Trigger.RejectDocument))
            {
                throw new InvalidOperationException("не могу провести операцию RejectDocument");
            }
        }
    }
}