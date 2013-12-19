using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invest.Common.Model.ProjectModels;
using Invest.Common.Model.ProjectWorkflow;
using Stateless;

namespace Invest.Common.State
{
    public class ProjectUpdateStateMachine
    {
        #region PrivateFields

        private readonly StateMachine<ProjectUpdatedStates, ProjectUpdatedTriggers> _stateMachine;
        private readonly string _userName;
        private readonly string[] _userRole;
        private readonly Project _currentProject;

        #endregion

        #region Configure

        public ProjectUpdateStateMachine()
        {

        }

        #endregion

        #region OnEntry Methods

        #endregion

        #region Guard Methods

        #endregion

        #region Configure

        private void Configure()
        {

        }

        #endregion
    }

    #region Project State Enums

    public enum ProjectUpdatedStates
    {
        Proposed = 0,
        OnMap = 1,
        InvestorResponsed = 2,
        InvestorApprove = 3,
        RequestPassing = 4,
        MileStonePassing = 5,
        Done = 6
    }

    public enum ProjectUpdatedTriggers
    {
        FillProject = 0,
        InvestorResponse = 1,
        InvestorApprove = 2,
        RequestUpdate = 3,
        MilestoneUpdate = 4
    }

    #endregion
}
