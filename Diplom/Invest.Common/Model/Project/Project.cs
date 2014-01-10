using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.Project
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof (GreenField), typeof (UnUsedBuilding), typeof (BrownField))]
    public class Project : IMongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

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

        [Display(Name = "Регион")]
        [Required(ErrorMessage = "Введите регион")]
        public string Region { get; set; }

        [Display(Name = "Теги")]
        public List<string> Tags { get; set; }

        [Display(Name = "Инвестор")]
        public string InvestorUser { get; set; }

        [Display(Name = "Задачи")]
        public List<ProjectTask> Tasks { get; set; }

        [Display(Name = "Шаги")]
        public List<Step> Steps { get; set; }

        [Display(Name = "Под проекты")]
        public List<Project> SubProject { get; set; }

        [Display(Name = "Состояние")]
        public Workflow WorkflowState { get; set; }

        [BsonIgnore]
        [Display(Name = "Тип проекта")]
        public string ProjectType
        {
            get { return GetType().Name; }
        }

        [BsonIgnore]
        public Comission ProjectComission
        {
            get
            {
                return
                    RepositoryContext.Current.GetOne<Comission>(
                        c => c.ProjectIds.Contains(_id) && c.Type == ComissionType.Comission);
            }
        }

        [BsonIgnore]
        public Comission ProjectIspolcom
        {
            get
            {
                return
                    RepositoryContext.Current.GetOne<Comission>(
                        c => c.ProjectIds.Contains(_id) && c.Type == ComissionType.Ispolcom);
            }
        }

        public List<InvestorResponse> Responses { get; set; }
    }
}