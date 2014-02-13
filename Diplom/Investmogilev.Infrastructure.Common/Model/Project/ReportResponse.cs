using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.User;
using MongoDB.Bson.Serialization.Attributes;

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
		public string ResponseUserId { get; set; }

		[BsonIgnore]
		public Users ResponseUser { get; set; }

		[BsonIgnore]
		public virtual Project Project { get; set; }

		[BsonIgnore]
		public virtual Report Report { get; set; }

		[BsonIgnore]
		public virtual ProjectTask Task { get; set; }

		[Display(Name = "Идентификатор отклика")]
		public string Id { get; set; }
	}
}