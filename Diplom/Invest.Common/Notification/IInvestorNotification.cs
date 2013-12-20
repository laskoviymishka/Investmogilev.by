﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invest.Common.Model.ProjectModels;

namespace Invest.Common.Notification
{
    public interface IInvestorNotification
    {
        void OnInvestorAproveEntry(Project project);
        void OnWaitForInvestorEntry(Project project);
        void OnOnReviewEntry(Project project);
        void OnOnComissionEntry(Project project);
        void OnRealizationEntry(Project project);
        void OnDoneEntry(Project project);

        void OnOnMapExit(Project project);
        void OnInvestorAproveExit(Project project);
        void OnWaitForInvestorExit(Project project);
        void OnOnReviewExit(Project project);
        void OnOnComissionExit(Project project);
        void OnRealizationExit(Project project);
        void OnDoneExit(Project project);
    }
}