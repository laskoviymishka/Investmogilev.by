using Investmogilev.Infrastructure.Common.State;

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow
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
        private readonly IUnitsOfWorkContainer _unitsOfWork;

        #endregion

        #region Constructor

        public ProjectWorkflowWrapper(ProjectWorkflow workflow,
            IUnitsOfWorkContainer unitsOfWork)
        {
            _workflow = workflow;
            _unitsOfWork = unitsOfWork;

            //Entry Methods Binding
            BindEntryMethods();

            //Exit Methods Binding
            BindExitMethods();

            //Guard For Transitions
            BindGuardMethods();
        }

        public ProjectWorkflowWrapper(ProjectWorkflow workflow)
        {
            _workflow = workflow;
        }

        #endregion

        #region Public Methods

        public void SetContext(string currentUser, string[] roles)
        {
        }

        public bool IsMoveablde(ProjectWorkflow.Trigger trigger)
        {
            return _workflow.CanFire(trigger);
        }

        public bool Move(ProjectWorkflow.Trigger trigger)
        {
            return _workflow.TryFireTrigger(trigger);
        }

        public ProjectWorkflow.State CurrentState { get { return _workflow.GetState; } }

        #endregion

        #region Binding

        private void BindGuardMethods()
        {
            _workflow.GuardClauseFromOnMapToOpenUsingTriggerReOpen =
                () => _unitsOfWork.OpenUoW.FromMapToOpen() && _unitsOfWork.OnMapUoW.FromOnMapToOpen();
            _workflow.GuardClauseFromOpenToOnMapUsingTriggerFillInformation =
                () => _unitsOfWork.OpenUoW.FromOpenToMap();

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
                _unitsOfWork.DocumentSendingUoW.CouldDocumentUpdate;
            _workflow.GuardClauseFromDocumentSendingToWaitInvolvedUsingTriggerDocumentUpdate =
                _unitsOfWork.DocumentSendingUoW.CouldDocumentUpdateAndLeave;

            _workflow.GuardClauseFromWaitInvolvedToInvolvedOrganizationsUsingTriggerFillInvolvedOrganization =
                _unitsOfWork.WaitInvolvedUoW.CouldFillInvolvedOrganization;

            _workflow.GuardClauseFromInvolvedOrganizationsToInvolvedOrganizationsUsingTriggerInvolvedOrganizationUpdate =
                _unitsOfWork.InvolvedorganizationsUoW.CouldInvolvedOrganizationUpdate;
            _workflow.GuardClauseFromInvolvedOrganizationsToWaitComissionUsingTriggerToComission =
                _unitsOfWork.InvolvedorganizationsUoW.CouldInvolvedOrganizationUpdateAndLeave;

            _workflow.GuardClauseFromWaitComissionToOnComissionUsingTriggerComission =
                _unitsOfWork.WaitComissionUoW.CouldComission;

            //Not Implemented
            _workflow.GuardClauseFromInMinEconomyToPlanCreatingUsingTriggerMinEconomyResponsed =
                _unitsOfWork.MinEconomyUoW.CouldMinEconomyResponsed;

            _workflow.GuardClauseFromOnComissionToWaitComissionFixesUsingTriggerComissionFix =
                _unitsOfWork.OnComissionUoW.CouldComissionFix;
            _workflow.GuardClauseFromOnComissionToWaitIspolcomUsingTriggerToIspolcom =
                _unitsOfWork.OnComissionUoW.CouldToIspolcom;

            _workflow.GuardClauseFromOnIspolcomToInMinEconomyUsingTriggerToMinEconomy =
                _unitsOfWork.OnIspolcomUoW.CouldToMinEconomy;
            _workflow.GuardClauseFromOnIspolcomToWaitIspolcomFixesUsingTriggerToIspolcomFix =
                _unitsOfWork.OnIspolcomUoW.CouldToIspolcomFix;

            _workflow.GuardClauseFromPlanCreatingToPlanCreatingUsingTriggerUpdatePlan =
                _unitsOfWork.PlanCreatingUoW.CouldUpdatePlan;
            _workflow.GuardClauseFromPlanCreatingToRealizationUsingTriggerApprovePlan =
                _unitsOfWork.PlanCreatingUoW.CouldApprovePlan;

            _workflow.GuardClauseFromRealizationToDoneUsingTriggerUpdateRealization =
                _unitsOfWork.RealizationUoW.CouldUpdateRealizationAndLeave;
            _workflow.GuardClauseFromRealizationToRealizationUsingTriggerUpdateRealization =
                _unitsOfWork.RealizationUoW.CouldUpdateRealization;

            _workflow.GuardClauseFromWaitComissionFixesToWaitComissionFixesUsingTriggerComissionFixUpdate =
                _unitsOfWork.ComissionFixesUoW.CouldUpdateComissionFix;
            _workflow.GuardClauseFromWaitComissionFixesToWaitIspolcomUsingTriggerComissionFixUpdate =
                _unitsOfWork.ComissionFixesUoW.CouldUpdateComissionFixAndLeave;

            _workflow.GuardClauseFromWaitIspolcomFixesToWaitIspolcomFixesUsingTriggerIspolcomFixUpdate =
                _unitsOfWork.IspolcomFixesUoW.CouldIspolcomFixUpdate;
            _workflow.GuardClauseFromWaitIspolcomFixesToWaitIspolcomUsingTriggerIspolcomFixUpdate =
                _unitsOfWork.IspolcomFixesUoW.CouldIspolcomFixUpdateAndLeave;

            _workflow.GuardClauseFromWaitIspolcomToOnIspolcomUsingTriggerIspolcom =
                _unitsOfWork.WaitIspolcomUoW.CouldToIspolcom;
        }

        private void BindExitMethods()
        {
            _workflow.OnOpenExit = _unitsOfWork.OpenUoW.OnOpenExit;
            _workflow.OnOnMapExit = _unitsOfWork.OnMapUoW.OnMapExit;
            _workflow.OnInvestorApproveExit = _unitsOfWork.InvestorApproveUoW.OnInvestorApproveExit;
            _workflow.OnDocumentSendingExit = _unitsOfWork.DocumentSendingUoW.OnDocumentSendingExit;
            _workflow.OnWaitInvolvedExit = _unitsOfWork.WaitInvolvedUoW.OnWaitInvolvedExit;
            _workflow.OnInvolvedOrganizationsExit = _unitsOfWork.InvolvedorganizationsUoW.OnInvolvedOrganizationsExit;
            _workflow.OnWaitComissionExit = _unitsOfWork.WaitComissionUoW.OnWaitComissionExit;

            //Not implemented
            _workflow.OnOnComissionExit = _unitsOfWork.OnComissionUoW.OnOnComissionExit;
            _workflow.OnWaitComissionFixesExit = _unitsOfWork.ComissionFixesUoW.OnWaitComissionFixesExit;
            _workflow.OnWaitIspolcomExit = _unitsOfWork.WaitIspolcomUoW.OnWaitIspolcomExit;
            _workflow.OnOnIspolcomExit = _unitsOfWork.OnIspolcomUoW.OnOnIspolcomExit;
            _workflow.OnWaitIspolcomFixesExit = _unitsOfWork.IspolcomFixesUoW.OnWaitIspolcomFixesExit;
            _workflow.OnInMinEconomyExit = _unitsOfWork.MinEconomyUoW.OnInMinEconomyExit;
            _workflow.OnPlanCreatingExit = _unitsOfWork.PlanCreatingUoW.OnPlanCreatingExit;
            _workflow.OnRealizationExit = _unitsOfWork.RealizationUoW.OnRealizationExit;
            _workflow.OnDoneExit = _unitsOfWork.DoneUoW.OnDoneExit;
        }

        private void BindEntryMethods()
        {
            _workflow.OnOpenEntry = _unitsOfWork.OpenUoW.OnOpenEntry;
            _workflow.OnOnMapEntry = _unitsOfWork.OnMapUoW.OnMapEntry;
            _workflow.OnInvestorApproveEntry = _unitsOfWork.InvestorApproveUoW.OnInvestorApproveEntry;
            _workflow.OnDocumentSendingEntry = _unitsOfWork.DocumentSendingUoW.OnDocumentSendingEntry;
            _workflow.OnWaitInvolvedEntry = _unitsOfWork.WaitInvolvedUoW.OnWaitInvolvedEntry;
            _workflow.OnInvolvedOrganizationsEntry = _unitsOfWork.InvolvedorganizationsUoW.OnInvolvedOrganizationsEntry;
            _workflow.OnWaitComissionEntry = _unitsOfWork.WaitComissionUoW.OnWaitComissionEntry;

            //Not Implemnted
            _workflow.OnOnComissionEntry = _unitsOfWork.OnComissionUoW.OnOnComissionEntry;
            _workflow.OnWaitComissionFixesEntry = _unitsOfWork.ComissionFixesUoW.OnWaitComissionFixesEntry;
            _workflow.OnWaitIspolcomEntry = _unitsOfWork.WaitIspolcomUoW.OnWaitIspolcomEntry;
            _workflow.OnOnIspolcomEntry = _unitsOfWork.OnIspolcomUoW.OnOnIspolcomEntry;
            _workflow.OnWaitIspolcomFixesEntry = _unitsOfWork.IspolcomFixesUoW.OnWaitIspolcomFixesEntry;
            _workflow.OnInMinEconomyEntry = _unitsOfWork.MinEconomyUoW.OnInMinEconomyEntry;
            _workflow.OnPlanCreatingEntry = _unitsOfWork.PlanCreatingUoW.OnPlanCreatingEntry;
            _workflow.OnRealizationEntry = _unitsOfWork.RealizationUoW.OnRealizationEntry;
            _workflow.OnDoneEntry = _unitsOfWork.DoneUoW.OnDoneEntry;
        }

        #endregion
    }
}
