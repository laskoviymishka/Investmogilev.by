// // -----------------------------------------------------------------------
// // <copyright file="ProjectStateContext.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow
{
	#region Using

	using System.Collections.Generic;
	using Investmogilev.Infrastructure.BusinessLogic.Notification;
	using Investmogilev.Infrastructure.Common.Model.Project;
	using Investmogilev.Infrastructure.Common.Repository;
	using Investmogilev.Infrastructure.Common.State.StateAttributes;

	#endregion

	public class ProjectStateContext : IStateContext
	{
		public Project CurrentProject { get; set; }
		public IRepository Repository { get; set; }
		public IUserNotification UserNotification { get; set; }
		public IAdminNotification AdminNotification { get; set; }
		public IInvestorNotification InvestorNotification { get; set; }
		public string UserName { get; set; }
		public IEnumerable<string> Roles { get; set; }
	}
}