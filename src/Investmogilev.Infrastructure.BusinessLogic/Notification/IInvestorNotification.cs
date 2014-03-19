// // -----------------------------------------------------------------------
// // <copyright file="IInvestorNotification.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	#region Using

	using Investmogilev.Infrastructure.Common.Model.Project;

	#endregion

	public interface IInvestorNotification
	{
		void InvestorResponsed(Project project);

		void DocumentUpdate(Project project);

		void ProjectAproved(Project project);

		void InvolvedOrganizationUpdate(Project project);

		void Comission(Comission comission, Project project);

		void WaitComission(Project project);

		void UpdateComissionFix(Project project);

		void ComissionFixNeeded(Project project);

		void WaitIspolcom(Project project);

		void OnIspolcom(Comission comission, Project project);

		void IspolcomFixNeeded(Project project);

		void UpdateIspolcomFix(Project project);

		void InMinEconomy(Project project);

		void MinEconomyResponsed(Project project);

		void Realization(Project project);

		void Done(Project project);
	}
}