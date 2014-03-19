// // -----------------------------------------------------------------------
// // <copyright file="UserNotification.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	#region Using

	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.State;

	#endregion

	public class UserNotification : BaseNotificate, IUserNotification
	{
		public UserNotification()
			: base(RepositoryContext.Current)
		{
		}

		public void NotificateOpen(Project currentProject)
		{
			SendMailFromDb(currentProject, currentProject, ProjectWorkflow.Trigger.ReOpen, UserType.User);
		}

		public void InvestorResponsed(Project currentProject)
		{
			SendMailFromDb(currentProject, currentProject, ProjectWorkflow.Trigger.InvestorResponsed, UserType.User);
		}
	}
}