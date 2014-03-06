using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Investmogilev.UI.Portal.Models
{
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
