// // -----------------------------------------------------------------------
// // <copyright file="LinkAdditionalInfo.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	public class LinkAdditionalInfo : AdditionalInfo
	{
		public string Link { get; set; }
		public string ParentSiteName { get; set; }
	}
}