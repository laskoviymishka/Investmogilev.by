using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Investmogilev.Infrastructure.Common.Model.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	public class Report : IMongoEntity
	{
		private ReportResponse _response;

		[Display(Name = "Тело отчета")]
		public string Body { get; set; }

		[Display(Name = "Идентификатор проекта")]
		public string ProjectId { get; set; }

		[Display(Name = "Идентификатор задачи")]
		public string TaskId { get; set; }

		[BsonIgnore]
		public string ReportResponseId { get; set; }

		[Display(Name = "Дополнительная информация")]
		public List<AdditionalInfo> Info { get; set; }

		[Display(Name = "Отклик")]
		public ReportResponse ReportResponse
		{
			get
			{
				if (ReportResponses != null)
				{
					return ReportResponses.FirstOrDefault();
				}
				return _response;
			}
			set { _response = value; }
		}

		[Display(Name = "Отклик")]
		public List<ReportResponse> ReportResponses { get; set; }

		[Display(Name = "Время созданя")]
		public DateTime ReportTime { get; set; }

		[BsonIgnore]
		public ProjectTask Task { get; set; }

		[BsonIgnore]
		public Project Project { get; set; }

		[Display(Name = "Идентификатор")]
		public string Id { get; set; }
	}
}