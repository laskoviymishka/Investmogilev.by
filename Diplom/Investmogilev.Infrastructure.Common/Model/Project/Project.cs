using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Investmogilev.Infrastructure.Common.Localization;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	[BsonIgnoreExtraElements]
	[BsonKnownTypes(typeof(GreenField), typeof(UnUsedBuilding), typeof(BrownField))]
	public class Project : IMongoEntity
	{
		private List<ProjectNotes> _notes;

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_Name_Имя_проекта")]
		[Required(ErrorMessageResourceType = typeof(LocalizationResource),
			ErrorMessageResourceName = "Project_Name_Введите_имя_проекта")]
		public string Name { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_Description_Описание_проекта")]
		[Required(ErrorMessageResourceType = typeof(LocalizationResource),
			ErrorMessageResourceName = "Project_Description_Введите_описание_проекта")]
		public string Description { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_Address_Координаты")]
		[Required(ErrorMessageResourceType = typeof(LocalizationResource),
			ErrorMessageResourceName = "Project_Address_Координаты_проекта_обязательны")]
		public Address Address { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_AddressName_Адрес")]
		[Required(ErrorMessageResourceType = typeof(LocalizationResource),
			ErrorMessageResourceName = "Project_AddressName_Введите_правильный_адрес")]
		public string AddressName { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_Region_Регион")]
		[Required(ErrorMessageResourceType = typeof(LocalizationResource),
			ErrorMessageResourceName = "Project_Region_Введите_регион")]
		public string Region { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_Tags_Теги")]
		public virtual List<Tag> Tags { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_InvestorUser_Инвестор")]
		public string InvestorUser { get; set; }

		[BsonIgnore]
		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_InvestorUser_Инвестор")]
		public Users Investor { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_Tasks_Задачи")]
		public virtual List<ProjectTask> Tasks { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_SubProject_Под_проекты")]
		public virtual List<Project> SubProject { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_WorkflowState_Состояние")]
		public virtual Workflow WorkflowState { get; set; }

		[BsonIgnore]
		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_ProjectType_Тип_проекта")]
		public string ProjectType
		{
			get { return GetType().Name; }
		}

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_Info_Дополнительная_информация")]
		public virtual List<AdditionalInfo> Info { get; set; }

		[BsonIgnore]
		public virtual Comission ProjectComission { get; set; }

		[BsonIgnore]
		public virtual List<ProjectNotes> ProjectNotes { get; set; }

		[BsonIgnore]
		public virtual Comission ProjectIspolcom { get; set; }

		public virtual List<InvestorResponse> Responses { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonIgnore]
		public string ProjectComissionId { get; set; }

		[BsonIgnore]
		public string ProjectIspolcomId { get; set; }
	}
}