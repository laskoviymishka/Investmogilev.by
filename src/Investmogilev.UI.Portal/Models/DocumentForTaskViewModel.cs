// // -----------------------------------------------------------------------
// // <copyright file="DocumentForTaskViewModel.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Models
{
	#region Using

	using Investmogilev.Infrastructure.Common.Model.Common;

	#endregion

	public class DocumentForTaskViewModel : DocumentAdditionalInfo
	{
		public DocumentForTaskViewModel()
		{
		}

		public DocumentForTaskViewModel(DocumentAdditionalInfo info)
		{
			_id = info._id;
			FilePath = info.FilePath;
			InfoName = info.InfoName;
			InfoValue = info.InfoValue;
		}

		public string ProjectId { get; set; }
		public string TaskId { get; set; }
	}
}