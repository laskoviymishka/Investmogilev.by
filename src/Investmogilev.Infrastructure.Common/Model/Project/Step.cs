// // -----------------------------------------------------------------------
// // <copyright file="Step.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	#region Using

	using System;
	using System.Collections.Generic;

	#endregion

	public class Step
	{
		public string Name { get; set; }
		public string Body { get; set; }
		public DateTime Milestone { get; set; }
		public DateTime PassedDate { get; set; }
		public bool IsDone { get; set; }
		public IEnumerable<Report> Reports { get; set; }
	}
}