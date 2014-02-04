using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
    public class ProjectNotes : IMongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [Display(Name = "Проект")]
        public string ProjectId { get; set; }

        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Введите заголовок")]
        public string NoteTitle { get; set; }

        [Display(Name = "Текст заметки")]
        [Required(ErrorMessage = "Введите текст")]
        public string NoteBody { get; set; }

        [Display(Name = "Автор")]
        public string CretorName { get; set; }

        [Display(Name = "Время создания")]
        public DateTime CreatedTime { get; set; }

        [Display(Name = "Доступна для ролей")]
        public List<string> RolesForView { get; set; }

        public IEnumerable<AdditionalInfo> NoteDocument { get; set; }
    }
}