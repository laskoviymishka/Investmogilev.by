using System;
using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Notification;
using Invest.Common.State;

namespace BusinessLogic.Wokflow
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
        private readonly IAdminNotification _adminNotificate;
        private readonly IUserNotification _userNotificationl;
        private readonly IUnitsOfWorkContainer _unitsOfWork;

        #endregion

        #region Constructor

        public ProjectWorkflowWrapper(ProjectWorkflow workflow,
            Project currentProject,
            IInvestorNotification investorNotificate,
            IAdminNotification adminNotification,
            IUserNotification userNotification,
            IUnitsOfWorkContainer unitsOfWork)
        {
            _investorNotificate = investorNotificate;
            _userNotificationl = userNotification;
            _adminNotificate = adminNotification;
            _workflow = workflow;
            _currentProject = currentProject;
            _unitsOfWork = unitsOfWork;

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

        private bool GuardClauseFromWaitIspolcomToOnIspolcomUsingTriggerIspolcom()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromWaitIspolcomFixesToWaitIspolcomUsingTriggerIspolcomFixUpdate()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromWaitIspolcomFixesToWaitIspolcomFixesUsingTriggerIspolcomFixUpdate()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromWaitIspolcomFixesToOnMapUsingTriggerRejectDocument()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromWaitInvolvedToInvolvedOrganizationsUsingTriggerFillInvolvedOrganization()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromWaitComissionToOnComissionUsingTriggerComission()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromWaitComissionFixesToWaitIspolcomUsingTriggerComissionFixUpdate()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromWaitComissionFixesToWaitComissionFixesUsingTriggerComissionFixUpdate()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromWaitComissionFixesToOnMapUsingTriggerRejectDocument()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromRealizationToRealizationUsingTriggerUpdateRealization()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromRealizationToDoneUsingTriggerUpdateRealization()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromPlanCreatingToRealizationUsingTriggerApprovePlan()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromPlanCreatingToPlanCreatingUsingTriggerUpdatePlan()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOpenToOnMapUsingTriggerFillInformation()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOnMapToOpenUsingTriggerReOpen()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOnMapToOnMapUsingTriggerUpdateInformation()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOnMapToInvestorAproveUsingTriggerInvestorResponsed()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOnIspolcomToWaitIspolcomFixesUsingTriggerToIspolcomFix()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOnIspolcomToOnMapUsingTriggerRejectDocument()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOnIspolcomToInMinEconomyUsingTriggerToMinEconomy()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOnComissionToWaitIspolcomUsingTriggerToIspolcom()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOnComissionToWaitComissionFixesUsingTriggerComissionFix()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromOnComissionToOnMapUsingTriggerRejectDocument()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromInvolvedOrganizationsToWaitComissionUsingTriggerToComission()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromInvolvedOrganizationsToInvolvedOrganizationsUsingTriggerInvolvedOrganizationUpdate()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromInvestorAproveToInvestorAproveUsingTriggerInvestorResponsed()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromInvestorAproveToDocumentSendingUsingTriggerInvestorSelected()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromInMinEconomyToPlanCreatingUsingTriggerMinEconomyResponsed()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromDocumentSendingToWaitInvolvedUsingTriggerDocumentUpdate()
        {
            throw new NotImplementedException();
        }

        private bool GuardClauseFromDocumentSendingToDocumentSendingUsingTriggerDocumentUpdate()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Entry Methods

        private void OnWaitIspolcomFixesEntry()
        {
            throw new NotImplementedException();
        }

        private void OnWaitIspolcomEntry()
        {
            throw new NotImplementedException();
        }

        private void OnWaitInvolvedEntry()
        {
            throw new NotImplementedException();
        }

        private void OnWaitComissionFixesEntry()
        {
            throw new NotImplementedException();
        }

        private void OnWaitComissionEntry()
        {
            throw new NotImplementedException();
        }

        private void OnRealizationEntry()
        {
            throw new NotImplementedException();
        }

        private void OnPlanCreatingEntry()
        {
            throw new NotImplementedException();
        }

        private void OnOpenEntry()
        {
            throw new NotImplementedException();
        }

        private void OnOnMapEntry()
        {
            throw new NotImplementedException();
        }

        private void OnOnIspolcomEntry()
        {
            throw new NotImplementedException();
        }

        private void OnOnComissionEntry()
        {
            throw new NotImplementedException();
        }

        private void OnInvolvedOrganizationsEntry()
        {
            throw new NotImplementedException();
        }

        private void OnInvestorAproveEntry()
        {
            throw new NotImplementedException();
        }

        private void OnInvestorApproveEntry()
        {
            throw new NotImplementedException();
        }

        private void OnDocumentSendingEntry()
        {
            throw new NotImplementedException();
        }

        private void OnInMinEconomyEntry()
        {
            throw new NotImplementedException();
        }

        private void OnDoneEntry()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Exit Methods

        private void OnWaitIspolcomFixesExit()
        {
            throw new NotImplementedException();
        }

        private void OnWaitIspolcomExit()
        {
            throw new NotImplementedException();
        }

        private void OnWaitInvolvedExit()
        {
            throw new NotImplementedException();
        }

        private void OnWaitComissionFixesExit()
        {
            throw new NotImplementedException();
        }

        private void OnWaitComissionExit()
        {
            throw new NotImplementedException();
        }

        private void OnRealizationExit()
        {
            throw new NotImplementedException();
        }

        private void OnPlanCreatingExit()
        {
            throw new NotImplementedException();
        }

        private void OnOpenExit()
        {
            throw new NotImplementedException();
        }

        private void OnOnMapExit()
        {
            throw new NotImplementedException();
        }

        private void OnOnIspolcomExit()
        {
            throw new NotImplementedException();
        }

        private void OnOnComissionExit()
        {
            throw new NotImplementedException();
        }

        private void OnInvolvedOrganizationsExit()
        {
            throw new NotImplementedException();
        }

        private void OnInvestorAproveExit()
        {
            throw new NotImplementedException();
        }

        private void OnInvestorApproveExit()
        {
            throw new NotImplementedException();
        }

        private void OnDocumentSendingExit()
        {
            throw new NotImplementedException();
        }

        private void OnInMinEconomyExit()
        {
            throw new NotImplementedException();
        }

        private void OnDoneExit()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Binding

        private void BindGuardMethods()
        {
            _workflow.GuardClauseFromOnMapToOpenUsingTriggerReOpen =
                () => _unitsOfWork.OpenUoW.FromMapToOpen() && _unitsOfWork.OnMapUoW.FromOnMapToOpen();
            _workflow.GuardClauseFromOpenToOnMapUsingTriggerFillInformation =
                () => _unitsOfWork.OpenUoW.FromOpenToMap() && _unitsOfWork.OnMapUoW.FromOpenToOnMap();

            _workflow.GuardClauseFromOnComissionToOnMapUsingTriggerRejectDocument =
                _unitsOfWork.OnMapUoW.FromOnComissionToOnMap;
            _workflow.GuardClauseFromOnIspolcomToOnMapUsingTriggerRejectDocument =
                _unitsOfWork.OnMapUoW.FromOnIspolcomToOnMap;
            _workflow.GuardClauseFromOnMapToOnMapUsingTriggerUpdateInformation =
                _unitsOfWork.OnMapUoW.FromOnMapToOnMap;
            _workflow.GuardClauseFromWaitComissionFixesToOnMapUsingTriggerRejectDocument =
                _unitsOfWork.OnMapUoW.FromWaitComissionFixesToOnMap;
            _workflow.GuardClauseFromWaitIspolcomFixesToOnMapUsingTriggerRejectDocument =
                _unitsOfWork.OnMapUoW.FromWaitIspolcomFixesToOnMap;
            _workflow.GuardClauseFromOnMapToInvestorApproveUsingTriggerInvestorResponsed =
                () => _unitsOfWork.OnMapUoW.FromOnMapToInvestorApprove()
                    && _unitsOfWork.InvestorApproveUoW.FromOnMapToInvestorApprove();

            _workflow.GuardClauseFromInvestorApproveToDocumentSendingUsingTriggerInvestorSelected =
                _unitsOfWork.InvestorApproveUoW.FromInvestorApproveToDocument;
            _workflow.GuardClauseFromInvestorApproveToInvestorApproveUsingTriggerInvestorResponsed =
                _unitsOfWork.InvestorApproveUoW.FromInvestorApproveToInvestorResponsed;



            _workflow.GuardClauseFromDocumentSendingToDocumentSendingUsingTriggerDocumentUpdate =
                GuardClauseFromDocumentSendingToDocumentSendingUsingTriggerDocumentUpdate;
            _workflow.GuardClauseFromDocumentSendingToWaitInvolvedUsingTriggerDocumentUpdate =
                GuardClauseFromDocumentSendingToWaitInvolvedUsingTriggerDocumentUpdate;
            _workflow.GuardClauseFromInMinEconomyToPlanCreatingUsingTriggerMinEconomyResponsed =
                GuardClauseFromInMinEconomyToPlanCreatingUsingTriggerMinEconomyResponsed;
            _workflow.GuardClauseFromInvolvedOrganizationsToInvolvedOrganizationsUsingTriggerInvolvedOrganizationUpdate =
                GuardClauseFromInvolvedOrganizationsToInvolvedOrganizationsUsingTriggerInvolvedOrganizationUpdate;
            _workflow.GuardClauseFromInvolvedOrganizationsToWaitComissionUsingTriggerToComission =
                GuardClauseFromInvolvedOrganizationsToWaitComissionUsingTriggerToComission;

            _workflow.GuardClauseFromOnComissionToWaitComissionFixesUsingTriggerComissionFix =
                GuardClauseFromOnComissionToWaitComissionFixesUsingTriggerComissionFix;
            _workflow.GuardClauseFromOnComissionToWaitIspolcomUsingTriggerToIspolcom =
                GuardClauseFromOnComissionToWaitIspolcomUsingTriggerToIspolcom;
            _workflow.GuardClauseFromOnIspolcomToInMinEconomyUsingTriggerToMinEconomy =
                GuardClauseFromOnIspolcomToInMinEconomyUsingTriggerToMinEconomy;

            _workflow.GuardClauseFromOnIspolcomToWaitIspolcomFixesUsingTriggerToIspolcomFix =
                GuardClauseFromOnIspolcomToWaitIspolcomFixesUsingTriggerToIspolcomFix;


            _workflow.GuardClauseFromPlanCreatingToPlanCreatingUsingTriggerUpdatePlan =
                GuardClauseFromPlanCreatingToPlanCreatingUsingTriggerUpdatePlan;
            _workflow.GuardClauseFromPlanCreatingToRealizationUsingTriggerApprovePlan =
                GuardClauseFromPlanCreatingToRealizationUsingTriggerApprovePlan;
            _workflow.GuardClauseFromRealizationToDoneUsingTriggerUpdateRealization =
                GuardClauseFromRealizationToDoneUsingTriggerUpdateRealization;
            _workflow.GuardClauseFromRealizationToRealizationUsingTriggerUpdateRealization =
                GuardClauseFromRealizationToRealizationUsingTriggerUpdateRealization;

            _workflow.GuardClauseFromWaitComissionFixesToWaitComissionFixesUsingTriggerComissionFixUpdate =
                GuardClauseFromWaitComissionFixesToWaitComissionFixesUsingTriggerComissionFixUpdate;
            _workflow.GuardClauseFromWaitComissionFixesToWaitIspolcomUsingTriggerComissionFixUpdate =
                GuardClauseFromWaitComissionFixesToWaitIspolcomUsingTriggerComissionFixUpdate;
            _workflow.GuardClauseFromWaitComissionToOnComissionUsingTriggerComission =
                GuardClauseFromWaitComissionToOnComissionUsingTriggerComission;
            _workflow.GuardClauseFromWaitInvolvedToInvolvedOrganizationsUsingTriggerFillInvolvedOrganization =
                GuardClauseFromWaitInvolvedToInvolvedOrganizationsUsingTriggerFillInvolvedOrganization;

            _workflow.GuardClauseFromWaitIspolcomFixesToWaitIspolcomFixesUsingTriggerIspolcomFixUpdate =
                GuardClauseFromWaitIspolcomFixesToWaitIspolcomFixesUsingTriggerIspolcomFixUpdate;
            _workflow.GuardClauseFromWaitIspolcomFixesToWaitIspolcomUsingTriggerIspolcomFixUpdate =
                GuardClauseFromWaitIspolcomFixesToWaitIspolcomUsingTriggerIspolcomFixUpdate;
            _workflow.GuardClauseFromWaitIspolcomToOnIspolcomUsingTriggerIspolcom =
                GuardClauseFromWaitIspolcomToOnIspolcomUsingTriggerIspolcom;
        }

        private void BindExitMethods()
        {
            _workflow.OnOpenExit = _unitsOfWork.OpenUoW.OnOpenExit;
            _workflow.OnOnMapExit = _unitsOfWork.OnMapUoW.OnMapExit;
            _workflow.OnInvestorApproveExit = _unitsOfWork.InvestorApproveUoW.OnInvestorApproveExit;


            _workflow.OnDoneExit = OnDoneExit;
            _workflow.OnDocumentSendingExit = OnDocumentSendingExit;
            _workflow.OnInMinEconomyExit = OnInMinEconomyExit;
            _workflow.OnInvestorApproveExit = OnInvestorApproveExit;
            _workflow.OnInvolvedOrganizationsExit = OnInvolvedOrganizationsExit;
            _workflow.OnOnComissionExit = OnOnComissionExit;
            _workflow.OnOnIspolcomExit = OnOnIspolcomExit;
            _workflow.OnPlanCreatingExit = OnPlanCreatingExit;
            _workflow.OnRealizationExit = OnRealizationExit;
            _workflow.OnWaitComissionExit = OnWaitComissionExit;
            _workflow.OnWaitComissionFixesExit = OnWaitComissionFixesExit;
            _workflow.OnWaitInvolvedExit = OnWaitInvolvedExit;
            _workflow.OnWaitIspolcomExit = OnWaitIspolcomExit;
            _workflow.OnWaitIspolcomFixesExit = OnWaitIspolcomFixesExit;
        }

        private void BindEntryMethods()
        {
            _workflow.OnOpenEntry = _unitsOfWork.OpenUoW.OnOpenEntry;
            _workflow.OnOnMapEntry = _unitsOfWork.OnMapUoW.OnMapEntry;
            _workflow.OnInvestorApproveEntry = _unitsOfWork.InvestorApproveUoW.OnInvestorApproveEntry;


            _workflow.OnDoneEntry = OnDoneEntry;
            _workflow.OnDocumentSendingEntry = OnDocumentSendingEntry;
            _workflow.OnInMinEconomyEntry = OnInMinEconomyEntry;
            _workflow.OnInvolvedOrganizationsEntry = OnInvolvedOrganizationsEntry;
            _workflow.OnOnComissionEntry = OnOnComissionEntry;
            _workflow.OnOnIspolcomEntry = OnOnIspolcomEntry;
            _workflow.OnPlanCreatingEntry = OnPlanCreatingEntry;
            _workflow.OnRealizationEntry = OnRealizationEntry;
            _workflow.OnWaitComissionEntry = OnWaitComissionEntry;
            _workflow.OnWaitComissionFixesEntry = OnWaitComissionFixesEntry;
            _workflow.OnWaitInvolvedEntry = OnWaitInvolvedEntry;
            _workflow.OnWaitIspolcomEntry = OnWaitIspolcomEntry;
            _workflow.OnWaitIspolcomFixesEntry = OnWaitIspolcomFixesEntry;
        }

        #endregion

        #endregion

        #region Private Helpers

        #endregion
    }
}
