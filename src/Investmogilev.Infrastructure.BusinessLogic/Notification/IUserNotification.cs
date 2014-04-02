// // -----------------------------------------------------------------------
// // <copyright file="IUserNotification.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	#region Using

	using Investmogilev.Infrastructure.Common.Model.Project;

	#endregion

	public interface IUserNotification
	{
		void NotificateOpen(Project currentProject);
		void InvestorResponsed(Project currentProject);
	}
}