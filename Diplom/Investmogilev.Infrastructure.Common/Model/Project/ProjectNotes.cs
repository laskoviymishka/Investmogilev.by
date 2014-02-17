using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Investmogilev.Infrastructure.Common.Localization;
using Investmogilev.Infrastructure.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	public class ProjectNotes : IMongoEntity
	{
		private Project _project;

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectNotes_ProjectId_Проект")]
		public string ProjectId { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectNotes_NoteTitle_Заголовок")]
		[Required(ErrorMessageResourceType = typeof (LocalizationResource),
			ErrorMessageResourceName = "ProjectNotes_NoteTitle_Введите_заголовок")]
		public string NoteTitle { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectNotes_NoteBody_Текст_заметки")]
		[Required(ErrorMessageResourceType = typeof (LocalizationResource),
			ErrorMessageResourceName = "ProjectNotes_NoteBody_Введите_текст")]
		public string NoteBody { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectNotes_CretorName_Автор")]
		public string CretorName { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectNotes_CreatedTime_Время_создания")]
		public DateTime CreatedTime { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "ProjectNotes_RolesForView_Доступна_для_ролей")]
		public string[] RolesForView { get; set; }

		public virtual List<AdditionalInfo> NoteDocument { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonIgnore]
		public virtual Project Project { get; set; }
	}
}