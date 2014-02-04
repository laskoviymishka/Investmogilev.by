using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Localization;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.State;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	public class ProjectTask : IMongoEntity
	{
		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask__id_Идентификатор_задачи")]
		public string _id { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_ProjectId_Идентификатор_проекта")]
		public string ProjectId { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_Title_Заголовок")]
		public string Title { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_TaskReport_Отчеты")]
		public List<Report> TaskReport { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_Type_Тип_задачи")]
		public TaskTypes Type { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_Step_Статус_проекта")]
		public ProjectWorkflow.State Step { get; set; } // Задача привязана к статусу проекта

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_Milestone_Необходимый_срок_выполнения")]
		public DateTime Milestone { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_CreationTime_Время_создания")]
		public DateTime CreationTime { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_CompleteTime_Время_выполнения")]
		public DateTime CompleteTime { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_IsComplete_Выполнена_")]
		public bool IsComplete { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectTask_Body_Описание_задачи")]
		public string Body { get; set; }
	}
}
