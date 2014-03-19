// // -----------------------------------------------------------------------
// // <copyright file="AddDocumentVIewModel.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Models
{
	#region Using

	using System.Collections.Generic;

	#endregion

	public class AddDocumentViewModel
	{
		public string PorjectId { get; set; }
		public List<string> Documents { get; set; }
	}
}