using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.State;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	public class ProjectTask : IMongoEntity
	{
		[Display(Name = "Идентификатор задачи")]
		public string _id { get; set; }

		[Display(Name = "Идентификатор проекта")]
		public string ProjectId { get; set; }

		[Display(Name = "Заголовок")]
		public string Title { get; set; }

		[Display(Name = "Отчеты")]
		public List<Report> TaskReport { get; set; }

		[Display(Name = "Тип задачи")]
		public TaskTypes Type { get; set; }

		[Display(Name = "Статус проекта")]
		public ProjectWorkflow.State Step { get; set; } // Задача привязана к статусу проекта

		[Display(Name = "Необходимый срок выполнения")]
		public DateTime Milestone { get; set; }

		[Display(Name = "Время создания")]
		public DateTime CreationTime { get; set; }

		[Display(Name = "Время выполнения")]
		public DateTime CompleteTime { get; set; }

		[Display(Name = "Выполнена?")]
		public bool IsComplete { get; set; }

		[Display(Name = "Описание задачи")]
		public string Body { get; set; }
	}
}
