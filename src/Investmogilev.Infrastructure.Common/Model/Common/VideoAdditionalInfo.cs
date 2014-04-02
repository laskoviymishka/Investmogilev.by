// // -----------------------------------------------------------------------
// // <copyright file="VideoAdditionalInfo.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	public class VideoAdditionalInfo : AdditionalInfo
	{
		public string VideoUrl { get; set; }
		public bool IsYoutube { get; set; }
	}
}