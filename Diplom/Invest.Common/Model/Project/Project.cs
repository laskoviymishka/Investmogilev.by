using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.Project
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof(GreenField), typeof(UnUsedBuilding), typeof(BrownField))]
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

        public List<ProjectTask> Tasks { get; set; }

        public List<Step> Steps { get; set; }

        public List<Project> SubProject { get; set; }

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
            get { return RepositoryContext.Current.GetOne<Comission>(c => c.ProjectIds.Contains(_id)); }
        }

        public List<InvestorResponse> Responses { get; set; }

    }
}