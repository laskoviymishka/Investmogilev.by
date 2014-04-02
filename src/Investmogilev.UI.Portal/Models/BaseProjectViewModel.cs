// // -----------------------------------------------------------------------
// // <copyright file="BaseProjectViewModel.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Models
{
	public class BaseProjectViewModel
	{
		public string _id { get; set; }
		public string Name { get; set; }
		public string Desctiption { get; set; }

		public string AsigneeId { get; set; }
	}
}