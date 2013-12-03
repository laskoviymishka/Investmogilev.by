using System;
using System.Collections.Generic;
using Invest.Common.Model.ProjectModels;
using Invest.Common.Model.User;
using MongoDB.Bson;
using MongoRepository;
using Stateless;

namespace Invest.Common.State
{
    public class ProjectStateMachine
    {
        #region PrivateFields

        private readonly StateMachine<ProjectStates, ProjectTriggers> _stateMachine;
        private readonly string _userName;
        private readonly string[] _userRole;
        private readonly Project _currentProject;

        #endregion

        #region Action On Entry

        public Action OnOpenEntry { get; set; }
        public Action OnWaitForAssigneeEntry { get; set; }
        public Action OnWaitForInvestorEntry { get; set; }
        public Action OnWaitForAdminInvesotApproveEntry { get; set; }
        public Action OnWaitForInvestorResponseApproveEntry { get; set; }
        public Action OnWaitForPlanEntry { get; set; }
        public Action OnWaitForAdminPlanApproveEntry { get; set; }
        public Action OnWaitForInvestorPlanApproveEntry { get; set; }
        public Action OnPlanRealiztionEntry { get; set; }
        public Action OnWaitForUserApproveCompletionEntry { get; set; }
        public Action OnWaitForAdminApproveCompletionEntry { get; set; }
        public Action OnWaitForDefectPlanEntry { get; set; }
        public Action OnWaitForAdminDefectPlanApproveEntry { get; set; }
        public Action OnWaitForInvestorDefectPlanApproveEntry { get; set; }
        public Action OnDefectPlanRealizationEntry { get; set; }
        public Action OnWaitForAdminApproveDefectPlanCompleteEntry { get; set; }
        public Action OnCloseProjectEntry { get; set; }

        #endregion

        #region Guard Action Before Entry

        public Func<bool> GuardOpenEntry { get; set; }
        public Func<bool> GuardFillProject { get; set; }
        public Func<bool> GuardAssigneeUser { get; set; }
        public Func<bool> GuardApproveAssignUser { get; set; }
        public Func<bool> GuardRejectAssignUser { get; set; }
        public Func<bool> GuardResponseFromInvest { get; set; }
        public Func<bool> GuardApproveInvestorByAdmin { get; set; }
        public Func<bool> GuardApproveResponseByInvestor { get; set; }
        public Func<bool> GuardCompleteFillPlan { get; set; }
        public Func<bool> GuardApprovePlanByAdmin { get; set; }
        public Func<bool> GuardApprovePlanByInvestor { get; set; }
        public Func<bool> GuardRejectPlanByInvestor { get; set; }
        public Func<bool> GuardCompletePlanRealization { get; set; }
        public Func<bool> GuardApprovePlanCompleteByUser { get; set; }
        public Func<bool> GuardApprovePlanCompleteByAdmin { get; set; }
        public Func<bool> GuardRejectPlanCompleteByUser { get; set; }
        public Func<bool> GuardRejectPlanCompleteByAdmin { get; set; }
        public Func<bool> GuardCompleteDefectPlan { get; set; }
        public Func<bool> GuardApproveCompleteDefectPlanByInvestor { get; set; }
        public Func<bool> GuardApproveCompleteDefectPlanByAdmin { get; set; }
        public Func<bool> GuardRejectCompleteDefectPlanByInvestor { get; set; }
        public Func<bool> GuardRejectCompleteDefectPlanByAdmin { get; set; }
        public Func<bool> GuardRejectInvestorByAdmin { get; set; }
        public Func<bool> GuardRejectResponseByInvestor { get; set; }
        public Func<bool> GuardRejectPlanByAdmin { get; set; }
        public Func<bool> GuardApproveDefectPlanByAdmin { get; set; }
        public Func<bool> GuardRejectDefectPlanByAdmin { get; set; }
        public Func<bool> GuardApproveDefectPlanByInvestor { get; set; }
        public Func<bool> GuardRejectDefectPlanByInvestor { get; set; }
        public Func<bool> GuardApproveCompleteDefectPlanByUser { get; set; }
        public Func<bool> GuardRejectCompleteDefectPlanByUser { get; set; }
        public Func<bool> GuardFillDefectPlan { get; set; }

        #endregion

        #region Action On Exit

        public Action OnOpenExit { get; set; }
        public Action OnWaitForAssigneeExit { get; set; }
        public Action OnWaitForInvestorExit { get; set; }
        public Action OnWaitForAdminInvesotApproveExit { get; set; }
        public Action OnWaitForInvestorResponseApproveExit { get; set; }
        public Action OnWaitForPlanExit { get; set; }
        public Action OnWaitForAdminPlanApproveExit { get; set; }
        public Action OnWaitForInvestorPlanApproveExit { get; set; }
        public Action OnPlanRealiztionExit { get; set; }
        public Action OnWaitForUserApproveCompletionExit { get; set; }
        public Action OnWaitForAdminApproveCompletionExit { get; set; }
        public Action OnWaitForDefectPlanExit { get; set; }
        public Action OnWaitForAdminDefectPlanApproveExit { get; set; }
        public Action OnWaitForInvestorDefectPlanApproveExit { get; set; }
        public Action OnDefectPlanRealizationExit { get; set; }
        public Action OnWaitForUserApproveDefectPlanExit { get; set; }
        public Action OnWaitForAdminApproveDefectPlanCompleteExit { get; set; }
        public Action OnCloseProjectExit { get; set; }

        #endregion

        #region Constructor

        public ProjectStateMachine(Project project, string userName, string[] userRole)
        {
            _currentProject = project;
            _userName = userName;
            _userRole = userRole;
            if (_currentProject.WorkflowState == null)
            {
                _currentProject.WorkflowState = new WorkflowEntity()
                    {
                        ChangeHistory = new List<History>(),
                        CurrenState = ProjectStates.Open
                    };
            }
            _stateMachine = new StateMachine<ProjectStates, ProjectTriggers>(_currentProject.WorkflowState.CurrenState);
            ApplyEntryMethods();
            ApplyExitMethods();
            ApplyGuards();
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(ProjectStates.Open)
                         .OnEntry(() => { if (OnOpenEntry != null) OnOpenEntry(); })
                         .OnExit(() => { if (OnOpenExit != null) OnOpenExit(); })
                         .PermitIf(ProjectTriggers.FillProject, ProjectStates.WaitForAssignee, GuardFillProject);

            _stateMachine.Configure(ProjectStates.WaitForAssignee)
                         .OnEntry(() => { if (OnWaitForAssigneeEntry != null) OnWaitForAssigneeEntry(); })
                         .OnExit(() => { if (OnWaitForAssigneeExit != null) OnWaitForAssigneeExit(); })
                         .PermitIf(ProjectTriggers.AssigneeUser, ProjectStates.WaitForInvestor, GuardAssigneeUser);

            _stateMachine.Configure(ProjectStates.WaitForInvestor)
                         .OnEntry(() => { if (OnWaitForInvestorEntry != null) OnWaitForInvestorEntry(); })
                         .OnExit(() => { if (OnWaitForInvestorExit != null) OnWaitForInvestorExit(); })
                         .PermitIf(ProjectTriggers.ResponseFromInvest, ProjectStates.WaitForAdminInvestorApprove,
                                   GuardResponseFromInvest);

            _stateMachine.Configure(ProjectStates.WaitForAdminInvestorApprove)
                         .OnEntry(() => { if (OnWaitForAdminInvesotApproveEntry != null) OnWaitForAdminInvesotApproveEntry(); })
                         .OnExit(() => { if (OnWaitForAdminInvesotApproveExit != null) OnWaitForAdminInvesotApproveExit(); })
                         .PermitIf(ProjectTriggers.ApproveInvestorByAdmin, ProjectStates.WaitForInvestorResponseApprove, GuardApproveInvestorByAdmin)
                         .PermitIf(ProjectTriggers.RejectInvestorByAdmin, ProjectStates.WaitForInvestor, GuardRejectInvestorByAdmin);

            _stateMachine.Configure(ProjectStates.WaitForInvestorResponseApprove)
                         .OnEntry(() => { if (OnWaitForInvestorResponseApproveEntry != null) OnWaitForInvestorResponseApproveEntry(); })
                         .OnExit(() => { if (OnWaitForInvestorResponseApproveExit != null) OnWaitForInvestorResponseApproveExit(); })
                         .PermitIf(ProjectTriggers.ApproveResponseByInvestor, ProjectStates.WaitForPlan, GuardApproveResponseByInvestor)
                         .PermitIf(ProjectTriggers.RejectResponseByInvestor, ProjectStates.WaitForInvestor, GuardRejectResponseByInvestor);

            _stateMachine.Configure(ProjectStates.WaitForPlan)
                         .OnEntry(() => { if (OnWaitForPlanEntry != null) OnWaitForPlanEntry(); })
                         .OnExit(() => { if (OnWaitForPlanExit != null) OnWaitForPlanExit(); })
                         .PermitIf(ProjectTriggers.CompleteFillPlan, ProjectStates.WaitForAdminPlanApprove, GuardCompleteFillPlan);

            _stateMachine.Configure(ProjectStates.WaitForAdminPlanApprove)
                         .OnEntry(() => { if (OnWaitForAdminPlanApproveEntry != null) OnWaitForAdminPlanApproveEntry(); })
                         .OnExit(() => { if (OnWaitForAdminPlanApproveExit != null) OnWaitForAdminPlanApproveExit(); })
                         .PermitIf(ProjectTriggers.ApprovePlanByAdmin, ProjectStates.WaitForInvestorPlanApprove, GuardApprovePlanByAdmin)
                         .PermitIf(ProjectTriggers.RejectPlanByAdmin, ProjectStates.WaitForPlan, GuardRejectPlanByAdmin);

            _stateMachine.Configure(ProjectStates.WaitForInvestorPlanApprove)
                         .OnEntry(() => { if (OnWaitForInvestorPlanApproveEntry != null) OnWaitForInvestorPlanApproveEntry(); })
                         .OnExit(() => { if (OnWaitForInvestorPlanApproveExit != null) OnWaitForInvestorPlanApproveExit(); })
                         .PermitIf(ProjectTriggers.ApprovePlanByInvestor, ProjectStates.PlanRealiztion, GuardApprovePlanByInvestor)
                         .PermitIf(ProjectTriggers.RejectPlanByInvestor, ProjectStates.WaitForPlan, GuardRejectPlanByInvestor);

            _stateMachine.Configure(ProjectStates.PlanRealiztion)
                         .OnEntry(() => { if (OnPlanRealiztionEntry != null) OnPlanRealiztionEntry(); })
                         .OnExit(() => { if (OnPlanRealiztionExit != null) OnPlanRealiztionExit(); })
                         .PermitIf(ProjectTriggers.CompletePlanRealization, ProjectStates.WaitForUserApproveCompletion, GuardCompletePlanRealization);

            _stateMachine.Configure(ProjectStates.WaitForUserApproveCompletion)
                         .OnEntry(() => { if (OnWaitForUserApproveCompletionEntry != null) OnWaitForUserApproveCompletionEntry(); })
                         .OnExit(() => { if (OnWaitForUserApproveCompletionExit != null) OnWaitForUserApproveCompletionExit(); })
                         .PermitIf(ProjectTriggers.ApprovePlanCompleteByUser, ProjectStates.WaitForAdminApproveCompletion, GuardApprovePlanCompleteByUser)
                         .PermitIf(ProjectTriggers.RejectPlanCompleteByUser, ProjectStates.WaitForDefectPlan, GuardRejectPlanCompleteByUser);

            _stateMachine.Configure(ProjectStates.WaitForAdminApproveCompletion)
                         .OnEntry(() => { if (OnWaitForAdminApproveCompletionEntry != null) OnWaitForAdminApproveCompletionEntry(); })
                         .OnExit(() => { if (OnWaitForAdminApproveCompletionExit != null) OnWaitForAdminApproveCompletionExit(); })
                         .PermitIf(ProjectTriggers.ApprovePlanCompleteByAdmin, ProjectStates.CloseProject, GuardApprovePlanCompleteByAdmin)
                         .PermitIf(ProjectTriggers.RejectPlanCompleteByAdmin, ProjectStates.WaitForDefectPlan, GuardRejectPlanCompleteByAdmin);

            _stateMachine.Configure(ProjectStates.WaitForDefectPlan)
                         .OnEntry(() => { if (OnWaitForDefectPlanEntry != null) OnWaitForDefectPlanEntry(); })
                         .OnExit(() => { if (OnWaitForDefectPlanExit != null) OnWaitForDefectPlanExit(); })
                         .PermitIf(ProjectTriggers.FillDefectPlan, ProjectStates.WaitForAdminDefectPlanApprove, GuardFillDefectPlan);

            _stateMachine.Configure(ProjectStates.WaitForAdminDefectPlanApprove)
                         .OnEntry(() => { if (OnWaitForAdminDefectPlanApproveEntry != null) OnWaitForAdminDefectPlanApproveEntry(); })
                         .OnExit(() => { if (OnWaitForAdminDefectPlanApproveExit != null) OnWaitForAdminDefectPlanApproveExit(); })
                         .PermitIf(ProjectTriggers.ApproveDefectPlanByAdmin, ProjectStates.WaitForInvestorDefectPlanApprove, GuardApproveDefectPlanByAdmin)
                         .PermitIf(ProjectTriggers.RejectDefectPlanByAdmin, ProjectStates.WaitForDefectPlan, GuardRejectDefectPlanByAdmin);

            _stateMachine.Configure(ProjectStates.WaitForInvestorDefectPlanApprove)
                         .OnEntry(() => { if (OnWaitForInvestorDefectPlanApproveEntry != null) OnWaitForInvestorDefectPlanApproveEntry(); })
                         .OnExit(() => { if (OnWaitForInvestorDefectPlanApproveExit != null) OnWaitForInvestorDefectPlanApproveExit(); })
                         .PermitIf(ProjectTriggers.ApproveDefectPlanByInvestor, ProjectStates.DefectPlanRealization, GuardApproveDefectPlanByInvestor)
                         .PermitIf(ProjectTriggers.RejectDefectPlanByInvestor, ProjectStates.WaitForDefectPlan, GuardRejectDefectPlanByInvestor);

            _stateMachine.Configure(ProjectStates.DefectPlanRealization)
                         .OnEntry(() => { if (OnDefectPlanRealizationEntry != null) OnDefectPlanRealizationEntry(); })
                         .OnExit(() => { if (OnDefectPlanRealizationExit != null) OnWaitForInvestorDefectPlanApproveExit(); })
                         .PermitIf(ProjectTriggers.CompleteDefectPlan, ProjectStates.WaitForUserApproveDefectPlanComplete, GuardCompleteDefectPlan);

            _stateMachine.Configure(ProjectStates.WaitForUserApproveDefectPlanComplete)
                         .OnEntry(() => { if (OnWaitForInvestorDefectPlanApproveEntry != null) OnWaitForInvestorDefectPlanApproveEntry(); })
                         .OnExit(() => { if (OnWaitForInvestorDefectPlanApproveExit != null) OnWaitForInvestorDefectPlanApproveExit(); })
                         .PermitIf(ProjectTriggers.ApproveCompleteDefectPlanByUser, ProjectStates.WaitForAdminApproveDefectPlanComplete, GuardApproveCompleteDefectPlanByUser)
                         .PermitIf(ProjectTriggers.RejectCompleteDefectPlanByUser, ProjectStates.WaitForDefectPlan, GuardRejectCompleteDefectPlanByUser);

            _stateMachine.Configure(ProjectStates.WaitForAdminApproveDefectPlanComplete)
                         .OnEntry(() => { if (OnWaitForAdminApproveDefectPlanCompleteEntry != null) OnWaitForAdminApproveDefectPlanCompleteEntry(); })
                         .OnExit(() => { if (OnWaitForAdminApproveDefectPlanCompleteExit != null) OnWaitForAdminApproveDefectPlanCompleteExit(); })
                         .PermitIf(ProjectTriggers.ApproveCompleteDefectPlanByAdmin, ProjectStates.CloseProject, GuardApproveCompleteDefectPlanByAdmin)
                         .PermitIf(ProjectTriggers.RejectCompleteDefectPlanByAdmin, ProjectStates.WaitForDefectPlan, GuardRejectCompleteDefectPlanByAdmin);

            _stateMachine.Configure(ProjectStates.CloseProject)
                 .OnEntry(() => { if (OnCloseProjectEntry != null) OnCloseProjectEntry(); })
                 .OnExit(() => { if (OnCloseProjectExit != null) OnCloseProjectExit(); });
        }


        private void ApplyEntryMethods()
        {
            OnOpenEntry = () => { };
            OnWaitForAssigneeEntry = () => MoveState(ProjectStates.WaitForAssignee);
            OnWaitForInvestorEntry = () => MoveState(ProjectStates.WaitForInvestor);
            OnWaitForAdminInvesotApproveEntry = () => MoveState(ProjectStates.WaitForAdminInvestorApprove);
            OnWaitForInvestorResponseApproveEntry = () => MoveState(ProjectStates.WaitForInvestorResponseApprove);
            OnWaitForPlanEntry = () => MoveState(ProjectStates.WaitForPlan);
            OnWaitForAdminPlanApproveEntry = () => MoveState(ProjectStates.WaitForAdminPlanApprove);
            OnWaitForInvestorPlanApproveEntry = () => MoveState(ProjectStates.WaitForInvestorPlanApprove);
            OnPlanRealiztionEntry = () => MoveState(ProjectStates.PlanRealiztion);
            OnWaitForUserApproveCompletionEntry = () => MoveState(ProjectStates.WaitForUserApproveCompletion);
            OnWaitForAdminApproveCompletionEntry = () => MoveState(ProjectStates.WaitForAdminApproveCompletion);
            OnWaitForDefectPlanEntry = () => MoveState(ProjectStates.WaitForDefectPlan);
            OnWaitForAdminDefectPlanApproveEntry = () => MoveState(ProjectStates.WaitForAdminDefectPlanApprove);
            OnWaitForInvestorDefectPlanApproveEntry = () => MoveState(ProjectStates.WaitForInvestorDefectPlanApprove);
            OnDefectPlanRealizationEntry = () => MoveState(ProjectStates.DefectPlanRealization);
            OnWaitForAdminApproveDefectPlanCompleteEntry = () => MoveState(ProjectStates.WaitForAdminApproveDefectPlanComplete);
            OnCloseProjectEntry = () => MoveState(ProjectStates.CloseProject);
        }

        private void ApplyExitMethods()
        {
            OnOpenExit = () => { };
            OnWaitForAssigneeExit = () => { };
            OnWaitForInvestorExit = () => { };
            OnWaitForAdminInvesotApproveExit = () => { };
            OnWaitForInvestorResponseApproveExit = () => { };
            OnWaitForPlanExit = () => { };
            OnWaitForAdminPlanApproveExit = () => { };
            OnWaitForInvestorPlanApproveExit = () => { };
            OnPlanRealiztionExit = () => { };
            OnWaitForUserApproveCompletionExit = () => { };
            OnWaitForAdminApproveCompletionExit = () => { };
            OnWaitForDefectPlanExit = () => { };
            OnWaitForAdminDefectPlanApproveExit = () => { };
            OnWaitForInvestorDefectPlanApproveExit = () => { };
            OnDefectPlanRealizationExit = () => { };
            OnWaitForAdminApproveDefectPlanCompleteExit = () => MoveState(ProjectStates.CloseProject);
            OnCloseProjectExit = () => { };
        }

        private void ApplyGuards()
        {
            GuardFillProject = () => true;
            GuardAssigneeUser = ApproveForAdmin;
            GuardApproveAssignUser = ApproveForUser;
            GuardRejectAssignUser = ApproveForUser;
            GuardResponseFromInvest = () => true;
            GuardApproveInvestorByAdmin = ApproveForAdmin;
            GuardApproveResponseByInvestor = ApproveForInvestor;
            GuardCompleteFillPlan = ApproveForUser;
            GuardApprovePlanByAdmin = ApproveForAdmin;
            GuardApprovePlanByInvestor = ApproveForInvestor;
            GuardCompletePlanRealization = ApproveForUser;
            GuardApprovePlanCompleteByUser = ApproveForUser;
            GuardApprovePlanCompleteByAdmin = ApproveForAdmin;
            GuardRejectPlanCompleteByUser = ApproveForUser;
            GuardRejectPlanCompleteByAdmin = ApproveForAdmin;
            GuardCompleteDefectPlan = ApproveForUser;
            GuardApproveCompleteDefectPlanByInvestor = ApproveForInvestor;
            GuardApproveCompleteDefectPlanByAdmin = ApproveForAdmin;
            GuardRejectCompleteDefectPlanByAdmin = ApproveForAdmin;
            GuardRejectCompleteDefectPlanByInvestor = ApproveForInvestor;
            GuardRejectInvestorByAdmin = ApproveForAdmin;
            GuardRejectResponseByInvestor = ApproveForInvestor;
            GuardRejectPlanByAdmin = ApproveForAdmin;
            GuardApproveDefectPlanByAdmin = ApproveForAdmin;
            GuardRejectDefectPlanByAdmin = ApproveForAdmin;
            GuardApproveDefectPlanByAdmin = ApproveForAdmin;
            GuardRejectDefectPlanByInvestor = ApproveForInvestor;
            GuardApproveCompleteDefectPlanByUser = ApproveForUser;
            GuardRejectCompleteDefectPlanByUser = ApproveForUser;
            GuardFillDefectPlan = ApproveForUser;
            GuardRejectPlanByInvestor = ApproveForInvestor;
            GuardApproveDefectPlanByInvestor = ApproveForInvestor;
        }

        #endregion

        #region Approvals

        private bool ApproveForAdmin()
        {
            foreach (string role in _userRole)
            {
                if (role == "Admin")
                {
                    return true;
                }
            }
            return false;
        }

        private bool ApproveForUser()
        {
            return _currentProject.AssignUser == _userName;
        }

        private bool ApproveForInvestor()
        {
            return _currentProject.InvestorUser == _userName;
        }

        #endregion

        private void MoveState(ProjectStates to)
        {
            var history = new History
                {
                    FromState = _currentProject.WorkflowState.CurrenState,
                    ToState = to,
                    Editor = _userName,
                    EditingTime = DateTime.Now
                };

            _currentProject.WorkflowState.CurrenState = to;
            if (_currentProject.WorkflowState.ChangeHistory == null)
            {
                _currentProject.WorkflowState.ChangeHistory = new List<History>();
            }

            _currentProject.WorkflowState.ChangeHistory.Add(history);
            RepositoryContext.Current.Update(_currentProject);

            NotificationQueueAdd(history, _currentProject.AssignUser);
            NotificationQueueAdd(history, _currentProject.InvestorUser);
        }

        private void NotificationQueueAdd(History history, string notificateUser)
        {
            var notification = new NotificationQueue
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    IsRead = false,
                    NotificationTime = DateTime.Now,
                    NotificationTitle = string.Format(
                        "Состояние проекта {0} изменилось с {1} на {2}",
                        _currentProject.Name,
                        history.FromState,
                        history.ToState),
                    NotigicationBody = string.Format(
                        "Состояние проекта {0} изменилось с {1} на {2} пользователем {3} в {4}",
                        _currentProject.Name,
                        history.FromState,
                        history.ToState,
                        _userName,
                        history.EditingTime),
                    UserName = notificateUser
                };

            RepositoryContext.Current.Add(notification);
        }


        public void Fire(ProjectTriggers trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }

    public enum ProjectStates
    {
        Open = 1,
        WaitForAssignee = 2,
        WaitForInvestor = 3,
        WaitForAdminInvestorApprove = 4,
        WaitForInvestorResponseApprove = 5,
        WaitForPlan = 6,
        WaitForAdminPlanApprove = 7,
        WaitForInvestorPlanApprove = 8,
        PlanRealiztion = 9,
        WaitForUserApproveCompletion = 10,
        WaitForAdminApproveCompletion = 11,
        WaitForDefectPlan = 12,
        WaitForAdminDefectPlanApprove = 13,
        WaitForInvestorDefectPlanApprove = 14,
        DefectPlanRealization = 15,
        WaitForUserApproveDefectPlanComplete = 16,
        WaitForAdminApproveDefectPlanComplete = 17,
        CloseProject = 18
    }

    public enum ProjectTriggers
    {
        FillProject,
        AssigneeUser,
        ApproveAssignUser,
        RejectAssignUser,
        ResponseFromInvest,
        ApproveInvestorByAdmin,
        RejectInvestorByAdmin,
        ApproveResponseByInvestor,
        RejectResponseByInvestor,
        CompleteFillPlan,
        ApprovePlanByAdmin,
        RejectPlanByAdmin,
        ApprovePlanByInvestor,
        RejectPlanByInvestor,
        CompletePlanRealization,
        ApprovePlanCompleteByUser,
        ApprovePlanCompleteByAdmin,
        RejectPlanCompleteByUser,
        RejectPlanCompleteByAdmin,
        FillDefectPlan,
        ApproveDefectPlanByAdmin,
        RejectDefectPlanByAdmin,
        ApproveDefectPlanByInvestor,
        RejectDefectPlanByInvestor,
        CompleteDefectPlan,
        ApproveCompleteDefectPlanByAdmin,
        ApproveCompleteDefectPlanByUser,
        RejectCompleteDefectPlanByAdmin,
        RejectCompleteDefectPlanByUser,
    }
}