using System.Collections.Generic;
using BusinessLogic.Notification;
using BusinessLogic.Wokflow.UnitsOfWork;
using Invest.Common.Model.Project;
using Invest.Common.Notification;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow
{
    public class UnitsOfWorkContainer : IUnitsOfWorkContainer
    {
        #region Protected Fields

        private readonly Project _currentProject;
        private readonly IRepository _repository;
        private readonly IUserNotification _userNotification;
        private readonly IAdminNotification _adminNotification;
        private readonly IInvestorNotification _investorNotification;
        private readonly string _userName;
        private readonly IEnumerable<string> _roles;

        #endregion

        public UnitsOfWorkContainer(Project currentProject,
            IRepository repository,
            IUserNotification userNotification,
            IAdminNotification adminNotification,
            IInvestorNotification investorNotification,
            string userName,
            IEnumerable<string> roles)
        {
            _currentProject = currentProject;
            _repository = repository;
            _userNotification = userNotification;
            _adminNotification = adminNotification;
            _investorNotification = investorNotification;
            _userName = userName;
            _roles = roles;

            OpenUoW = new OpenUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            OnMapUoW = new OnMapUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            InvestorApproveUoW = new InvestorApproveUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);

            WaitIspolcomUoW = new WaitIspolcomUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            WaitInvolvedUoW = new WaitInvolvedUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            WaitComissionUoW = new WaitComissionUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            RealizationUoW = new RealizationUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            PlanCreatingUoW = new PlanCreatingUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            OnIspolcomUoW = new OnIspolcomUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            OnComissionUoW = new OnComissionUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            MinEconomyUoW = new MinEconomyUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            IspolcomFixesUoW = new IspolcomFixesUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            InvolvedorganizationsUoW = new InvolvedorganizationsUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            DocumentSendingUoW = new DocumentSendingUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            ComissionFixesUoW = new ComissionFixesUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
            DoneUoW = new DoneUoW(_currentProject,
                _repository,
                _userNotification,
                _adminNotification,
                _investorNotification,
                _userName,
                _roles);
        }

        public IComissionFixesUoW ComissionFixesUoW { get; private set; }
        public IDocumentSendingUoW DocumentSendingUoW { get; private set; }
        public IDoneUoW DoneUoW { get; private set; }
        public IInvestorApproveUoW InvestorApproveUoW { get; private set; }
        public IInvolvedorganizationsUoW InvolvedorganizationsUoW { get; private set; }
        public IIspolcomFixesUoW IspolcomFixesUoW { get; private set; }
        public IMinEconomyUoW MinEconomyUoW { get; private set; }
        public IOnComissionUoW OnComissionUoW { get; private set; }
        public IOnIspolcomUoW OnIspolcomUoW { get; private set; }
        public IOnMapUoW OnMapUoW { get; private set; }
        public IOpenUoW OpenUoW { get; private set; }
        public IPlanCreatingUoW PlanCreatingUoW { get; private set; }
        public IRealizationUoW RealizationUoW { get; private set; }
        public IWaitComissionUoW WaitComissionUoW { get; private set; }
        public IWaitInvolvedUoW WaitInvolvedUoW { get; private set; }
        public IWaitIspolcomUoW WaitIspolcomUoW { get; private set; }
    }
}