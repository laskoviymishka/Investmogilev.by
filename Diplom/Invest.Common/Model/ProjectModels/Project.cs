using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace Invest.Common.Model.ProjectModels
{
    [BsonKnownTypes(typeof(GreenField), typeof(BrownField), typeof(UnUsedBuilding))]
    public class Project : IMongoEntity
    {
        private string _investuser;
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id
        {
            get;
            set;
        }

        [Display(Name = "Имя проекта")]
        [Required(ErrorMessage = "Введите имя проекта")]
        public string Name { get; set; }

        [Display(Name = "Описание проекта")]
        [Required(ErrorMessage = "Введите описание проекта")]
        public string Description { get; set; }

        [Display(Name = "Координаты")]
        [Required(ErrorMessage = "Координаты проекта обязательны")]
        public Address Address { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Введите правильный адрес")]
        public string AddressName { get; set; }

        [Display(Name = "Контакт")]
        [Required(ErrorMessage = "Введите контактные данные")]
        public string Contact { get; set; }

        [Display(Name = "Регион")]
        [Required(ErrorMessage = "Введите регион")]
        public string Region { get; set; }
        public string[] Mentors { get; set; }

        [Display(Name = "Ответственные")]
        public string AssignUser { get; set; }

        public string WorkflowId { get; set; }

        [Display(Name = "Теги")]
        public List<string> Tags { get; set; }

        [Display(Name = "Инвестор")]
        public string InvestorUser
        {
            get
            {
                if (_investuser == null)
                {
                    return string.Empty;
                }
                return _investuser;
            }
            set { _investuser = value; }
        }

        [BsonIgnore]
        public IEnumerable<Task> Tasks
        {
            get
            {
                return RepositoryContext.Current.All<Task>(t => t.ParentId == _id);
            }
        }

        public WorkflowEntity WorkflowState { get; set; }

        [BsonIgnore]
        [Display(Name = "Тип проекта")]
        public string ProjectType
        {
            get { return this.GetType().Name; }
        }

        public List<InvestorResponse> Responses { get; set; }

        [BsonIgnore]
        public IEnumerable<ProjectNotes> Notes
        {
            get
            {
                return RepositoryContext.Current.All<ProjectNotes>(t => t.ProjectId == _id);
            }
        }
    }
}
