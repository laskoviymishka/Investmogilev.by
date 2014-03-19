// // -----------------------------------------------------------------------
// // <copyright file="EditParametrViewModel.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	#region Using

	using System.Collections.Generic;

	#endregion

	public class EditParametrViewModel
	{
		public string RegionId { get; set; }
		public string ParametrName { get; set; }
		public IEnumerable<KeyValuePair<int, double>> Values { get; set; }
	}
}