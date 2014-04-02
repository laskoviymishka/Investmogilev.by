// // -----------------------------------------------------------------------
// // <copyright file="IssueViewModel.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Models
{
	#region Using

	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	#endregion

	public class IssueViewModel
	{
		[Required(ErrorMessage = "Заголовок обязателен")]
		[Display(Name = "Заголовок")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Описание обязательно")]
		[Display(Name = "Подробное описание")]
		public string Body { get; set; }

		[Required(ErrorMessage = "Необходима хотя бы одна категория")]
		[Display(Name = "Категория")]
		public List<string> Labels { get; set; }

		public string BaseUri { get; set; }
	}
}