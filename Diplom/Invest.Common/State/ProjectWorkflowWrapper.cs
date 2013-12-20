using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invest.Common.Model.Project;
using Invest.Common.Notification;

namespace Invest.Common.State
{
    public class ProjectWorkflowWrapper
    {
        #region Constatnt

        private const string ADMIN_ROLE = "Admin";
        private const string INVESTOR_ROLE = "Investor";
        private const string RAYON_ROLE = "User";

        #endregion

        #region Private Fields

        private readonly ProjectWorkflow _workflow;
        private readonly Project _currentProject;
        private string _currentUser;
        private IList<string> _roles;
        private readonly IInvestorNotification _investorNotificate;

        #endregion

        #region Constructor

        public ProjectWorkflowWrapper(ProjectWorkflow workflow, Project currentProject, IInvestorNotification investorNotificate)
        {
            _investorNotificate = investorNotificate;
            _workflow = workflow;
            _currentProject = currentProject;

            //Entry Methods Binding
            BindEntryMethods();

            //Exit Methods Binding
            BindExitMethods();

            //Guard For Transitions
            BindGuardMethods();
        }

        #endregion

        #region Public Methods

        public void SetContext(string currentUser, string[] roles)
        {
            _currentUser = currentUser;
            _roles = roles;
        }

        public bool TryMove(ProjectWorkflow.Trigger trigger)
        {
            return _workflow.TryFireTrigger(trigger);
        }

        public ProjectWorkflow.State CurrentState { get { return _workflow.GetState; } }

        #endregion

        #region Binding Methods

        #region Guards

        private bool GuardClauseFromWaitForInvestorToOnReviewUsingTriggerInvestorActivate()
        {
            return IsInvestorForProject;
        }

        private bool GuardClauseFromWaitForInvestorToOnMapUsingTriggerReInvestor()
        {
            return IsInvestorForProject || IsAdmin;
        }

        private bool GuardClauseFromRealizationToRealizationUsingTriggerUpdateProgress()
        {
            return IsInvestorForProject || IsAdmin;
        }

        private bool GuardClauseFromRealizationToOnReviewUsingTriggerReReview()
        {
            return IsAdmin || IsAssignedUser;
        }

        private bool GuardClauseFromRealizationToDoneUsingTriggerUpdateProgress()
        {
            return IsAdmin || IsAssignedUser;
        }

        private bool GuardClauseFromOpenToOnMapUsingTriggerFillInformation()
        {
            return IsAssignedUser;
        }

        private bool GuardClauseFromOnReviewToOnReviewUsingTriggerUpdateReviewProgress()
        {
            return IsAssignedUser || IsAdmin || IsInvestorForProject;
        }

        private bool GuardClauseFromOnReviewToOnMapUsingTriggerReInvestor()
        {
            return IsAdmin || IsAssignedUser;
        }

        private bool GuardClauseFromOnReviewToOnComissionUsingTriggerUpdateReviewProgress()
        {
            return IsAssignedUser || IsAdmin;
        }

        private bool GuardClauseFromOnMapToOpenUsingTriggerReOpen()
        {
            return IsAdmin;
        }

        private bool GuardClauseFromOnMapToOnMapUsingTriggerUpdateInformation()
        {
            return IsAssignedUser || IsAdmin;
        }

        private bool GuardClauseFromOnMapToInvestorAproveUsingTriggerInvestorResponsed()
        {
            return true;
        }

        private bool GuardClauseFromOnComissionToRealizationUsingTriggerComissionApprove()
        {
            return IsAdmin;
        }

        private bool GuardClauseFromOnComissionToOnReviewUsingTriggerReReview()
        {
            return IsAdmin;
        }

        private bool GuardClauseFromOnComissionToOnMapUsingTriggerReInvestor()
        {
            return IsAdmin;
        }

        private bool GuardClauseFromInvestorAproveToWaitForInvestorUsingTriggerInvestorSelected()
        {
            return IsAdmin || IsAssignedUser;
        }

        private bool GuardClauseFromInvestorAproveToInvestorAproveUsingTriggerInvestorResponsed()
        {
            return true;
        }

        private bool GuardClauseFromDoneToRealizationUsingTriggerReRealization()
        {
            return IsAdmin;
        }

        private bool GuardClauseFromDoneToOpenUsingTriggerReOpen()
        {
            return IsAdmin;
        }

        private bool GuardClauseFromDoneToOnReviewUsingTriggerReReview()
        {
            return IsAdmin;
        }

        private bool GuardClauseFromDoneToOnMapUsingTriggerReInvestor()
        {
            return IsAdmin;
        }

        #endregion

        #region Entry Methods

        private void OnOpenEntry()
        {
            //Проект в начальной стадии
            //TO-DO
            //Уведомить соответутсвующий район
            //Назначить ответственного
            //Уведомить администрацию
        }

        private void OnOnMapEntry()
        {
            //Проект размещен на карте
            //TO-DO
            //Уведомить администрацию
            //--Возможно уведомить заинтересованных существующих инвесторов
        }

        private void OnInvestorAproveEntry()
        {
            //На проект пришел минимум один отклик
            //TO-DO
            //Уведомить ответственного о необходимости проверить инвестора
            //Уведомить администратора

            //Отправить инвестору логин и пароль.
            _investorNotificate.OnInvestorAproveEntry(_currentProject);
        }

        private void OnWaitForInvestorEntry()
        {
            //Инвестор одобрен, необходимо кликнуть на ссылку
            //TO-DO

            //Уведомить инвестора, что он одобрен и необходимо кликнуть на ссылку
            _investorNotificate.OnWaitForInvestorEntry(_currentProject);

            //Уведомить администратора что этот инвестор одобрен
        }

        private void OnOnReviewEntry()
        {
            //Причастные структуры
            //TO-DO

            //Уведомить инвестора о текущем прогрессе
            _investorNotificate.OnOnReviewEntry(_currentProject);
        }

        private void OnOnComissionEntry()
        {
            //Причастные структуры пройдены, комиссия по рассмотрению началась
            //TO-DO

            //Уведомить инвестора о комиссии
            _investorNotificate.OnOnComissionEntry(_currentProject);

            //Уведомить администраторов о комиссии
        }

        private void OnRealizationEntry()
        {
            //Реализация проекта
            //TO-DO

            //Уведомить инвестора о одобренных отчетах
            _investorNotificate.OnRealizationEntry(_currentProject);

            //Уведомить администратора о новых отчетах
        }

        private void OnDoneEntry()
        {
            //Окончание проекта
            //TO-DO

            //Уведомить инвестора о завершении проекта
            _investorNotificate.OnDoneEntry(_currentProject);

            //Уведомить администраторов о завершении проекта
            //Уведомить район о завершении проекта
        }

        #endregion

        #region Exit Methods

        private void OnOpenExit()
        {
        }

        private void OnOnMapExit()
        {
            //Первый инвестор откликнулся
            //TO-DO
            //Уведомить о первом инвесторе район и администрацию

            _investorNotificate.OnOnMapExit(_currentProject);
        }

        private void OnInvestorAproveExit()
        {

            _investorNotificate.OnInvestorAproveExit(_currentProject);
        }

        private void OnWaitForInvestorExit()
        {
            _investorNotificate.OnWaitForInvestorExit(_currentProject);
        }

        private void OnOnReviewExit()
        {
            _investorNotificate.OnOnReviewExit(_currentProject);
        }

        private void OnOnComissionExit()
        {
            _investorNotificate.OnOnComissionExit(_currentProject);
        }

        private void OnRealizationExit()
        {
            _investorNotificate.OnRealizationExit(_currentProject);
        }

        private void OnDoneExit()
        {
            _investorNotificate.OnDoneExit(_currentProject);
        }

        #endregion

        #region Binding

        private void BindGuardMethods()
        {
            _workflow.GuardClauseFromDoneToOnMapUsingTriggerReInvestor = GuardClauseFromDoneToOnMapUsingTriggerReInvestor;
            _workflow.GuardClauseFromDoneToOnReviewUsingTriggerReReview = GuardClauseFromDoneToOnReviewUsingTriggerReReview;
            _workflow.GuardClauseFromDoneToOpenUsingTriggerReOpen = GuardClauseFromDoneToOpenUsingTriggerReOpen;
            _workflow.GuardClauseFromDoneToRealizationUsingTriggerReRealization = GuardClauseFromDoneToRealizationUsingTriggerReRealization;
            _workflow.GuardClauseFromInvestorAproveToInvestorAproveUsingTriggerInvestorResponsed = GuardClauseFromInvestorAproveToInvestorAproveUsingTriggerInvestorResponsed;
            _workflow.GuardClauseFromInvestorAproveToWaitForInvestorUsingTriggerInvestorSelected = GuardClauseFromInvestorAproveToWaitForInvestorUsingTriggerInvestorSelected;
            _workflow.GuardClauseFromOnComissionToOnMapUsingTriggerReInvestor = GuardClauseFromOnComissionToOnMapUsingTriggerReInvestor;
            _workflow.GuardClauseFromOnComissionToOnReviewUsingTriggerReReview = GuardClauseFromOnComissionToOnReviewUsingTriggerReReview;
            _workflow.GuardClauseFromOnComissionToRealizationUsingTriggerComissionApprove = GuardClauseFromOnComissionToRealizationUsingTriggerComissionApprove;
            _workflow.GuardClauseFromOnMapToInvestorAproveUsingTriggerInvestorResponsed = GuardClauseFromOnMapToInvestorAproveUsingTriggerInvestorResponsed;
            _workflow.GuardClauseFromOnMapToOnMapUsingTriggerUpdateInformation = GuardClauseFromOnMapToOnMapUsingTriggerUpdateInformation;
            _workflow.GuardClauseFromOnMapToOpenUsingTriggerReOpen = GuardClauseFromOnMapToOpenUsingTriggerReOpen;
            _workflow.GuardClauseFromOnReviewToOnComissionUsingTriggerUpdateReviewProgress = GuardClauseFromOnReviewToOnComissionUsingTriggerUpdateReviewProgress;
            _workflow.GuardClauseFromOnReviewToOnMapUsingTriggerReInvestor = GuardClauseFromOnReviewToOnMapUsingTriggerReInvestor;
            _workflow.GuardClauseFromOnReviewToOnReviewUsingTriggerUpdateReviewProgress = GuardClauseFromOnReviewToOnReviewUsingTriggerUpdateReviewProgress;
            _workflow.GuardClauseFromOpenToOnMapUsingTriggerFillInformation = GuardClauseFromOpenToOnMapUsingTriggerFillInformation;
            _workflow.GuardClauseFromRealizationToDoneUsingTriggerUpdateProgress = GuardClauseFromRealizationToDoneUsingTriggerUpdateProgress;
            _workflow.GuardClauseFromRealizationToOnReviewUsingTriggerReReview = GuardClauseFromRealizationToOnReviewUsingTriggerReReview;
            _workflow.GuardClauseFromRealizationToRealizationUsingTriggerUpdateProgress = GuardClauseFromRealizationToRealizationUsingTriggerUpdateProgress;
            _workflow.GuardClauseFromWaitForInvestorToOnMapUsingTriggerReInvestor = GuardClauseFromWaitForInvestorToOnMapUsingTriggerReInvestor;
            _workflow.GuardClauseFromWaitForInvestorToOnReviewUsingTriggerInvestorActivate = GuardClauseFromWaitForInvestorToOnReviewUsingTriggerInvestorActivate;
        }

        private void BindExitMethods()
        {
            _workflow.OnDoneExit = OnDoneExit;
            _workflow.OnInvestorAproveExit = OnInvestorAproveExit;
            _workflow.OnOnComissionExit = OnOnComissionExit;
            _workflow.OnOnMapExit = OnOnMapExit;
            _workflow.OnOnReviewExit = OnOnReviewExit;
            _workflow.OnOpenExit = OnOpenExit;
            _workflow.OnRealizationExit = OnRealizationExit;
            _workflow.OnWaitForInvestorExit = OnWaitForInvestorExit;
        }

        private void BindEntryMethods()
        {
            _workflow.OnDoneEntry = OnDoneEntry;
            _workflow.OnInvestorAproveEntry = OnInvestorAproveEntry;
            _workflow.OnOnComissionEntry = OnOnComissionEntry;
            _workflow.OnOnMapEntry = OnOnMapEntry;
            _workflow.OnOnReviewEntry = OnOnReviewEntry;
            _workflow.OnOpenEntry = OnOpenEntry;
            _workflow.OnRealizationEntry = OnRealizationEntry;
            _workflow.OnWaitForInvestorEntry = OnWaitForInvestorEntry;
        }
        #endregion

        #endregion

        #region Private Helpers

        private bool IsInvestorForProject
        {
            get { return (_currentUser == _currentProject.InvestorUser && _roles.Contains(INVESTOR_ROLE)); }
        }

        private bool IsAdmin
        {
             get { return _roles.Contains(ADMIN_ROLE);}
        }

        private bool IsAssignedUser
        {
            get { return true; }
        }

        #endregion
    }
}
