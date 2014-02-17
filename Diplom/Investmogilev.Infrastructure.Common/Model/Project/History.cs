﻿using System;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Localization;
using Investmogilev.Infrastructure.Common.Model.User;
using Investmogilev.Infrastructure.Common.State;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	public class History
	{
		public int ID { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "History_Editor_Кем_изменено")]
		public string EditorId { get; set; }

		[BsonIgnore]
		[Display(ResourceType = typeof(LocalizationResource), Name = "History_Editor_Кем_изменено")]
		public virtual Users Editor { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "History_From_Исходное_состояние")]
		public ProjectWorkflow.State From { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "History_To_Конечное_состояние")]
		public ProjectWorkflow.State To { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "History_Body_Дополнительная_информация")]
		public string Body { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "History_EditingTime_Дата_изменений")]
		public DateTime EditingTime { get; set; }
	}
}