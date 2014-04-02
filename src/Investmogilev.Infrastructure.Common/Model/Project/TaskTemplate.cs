// // -----------------------------------------------------------------------
// // <copyright file="TaskTemplate.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	#region Using

	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.State;

	#endregion

	public class TaskTemplate : IMongoEntity
	{
		public string Title { get; set; }
		public TaskTypes Type { get; set; }
		public ProjectWorkflow.State Step { get; set; }
		public string Body { get; set; }
		public AdditionalInfo Info { get; set; }
		public string _id { get; set; }
	}
}