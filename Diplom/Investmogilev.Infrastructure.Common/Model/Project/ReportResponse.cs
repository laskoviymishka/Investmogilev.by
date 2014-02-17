using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Model.Common;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	public class ReportResponse : IMongoEntity
	{
		[Display(Name = "Тело отклика")]
		public string Body { get; set; }

		[Display(Name = "Дополнительная информация")]
		public List<AdditionalInfo> Info { get; set; }

		[Display(Name = "Одобрен?")]
		public bool IsApproved { get; set; }

		[Display(Name = "Время отклика")]
		public DateTime ResponseTime { get; set; }

		[Display(Name = "Идентификатор проекта")]
		public string ProjectId { get; set; }

		[Display(Name = "Идентификатор отчета")]
		public string ReportId { get; set; }

		[Display(Name = "Идентификатор задачи")]
		public string TaskId { get; set; }

		[Display(Name = "Создатель отклика")]
		public string ResponseUser { get; set; }

		[Display(Name = "Идентификатор отклика")]
		public string Id { get; set; }
	}
}