using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Model.Common;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	public class Report : IMongoEntity
	{
		[Display(Name = "Тело отчета")]
		public string Body { get; set; }

		[Display(Name = "Идентификатор проекта")]
		public string ProjectId { get; set; }

		[Display(Name = "Идентификатор задачи")]
		public string TaskId { get; set; }

		[Display(Name = "Дополнительная информация")]
		public List<AdditionalInfo> Info { get; set; }

		[Display(Name = "Отклик")]
		public ReportResponse ReportResponse { get; set; }

		[Display(Name = "Время созданя")]
		public DateTime ReportTime { get; set; }

		[Display(Name = "Идентификатор")]
		public string _id { get; set; }
	}
}
