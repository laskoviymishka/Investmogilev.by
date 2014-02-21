using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Investmogilev.Infrastructure.Common.Localization;
using Investmogilev.Infrastructure.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	[BsonIgnoreExtraElements]
	[BsonKnownTypes(typeof (GreenField), typeof (UnUsedBuilding), typeof (BrownField))]
	public class Project : IMongoEntity
	{
		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_Name_Имя_проекта")]
		[Required(ErrorMessageResourceType = typeof (LocalizationResource),
			ErrorMessageResourceName = "Project_Name_Введите_имя_проекта")]
		public string Name { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_Description_Описание_проекта")]
		[Required(ErrorMessageResourceType = typeof (LocalizationResource),
			ErrorMessageResourceName = "Project_Description_Введите_описание_проекта")]
		public string Description { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_Address_Координаты")]
		[Required(ErrorMessageResourceType = typeof (LocalizationResource),
			ErrorMessageResourceName = "Project_Address_Координаты_проекта_обязательны")]
		public Address Address { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_AddressName_Адрес")]
		[Required(ErrorMessageResourceType = typeof (LocalizationResource),
			ErrorMessageResourceName = "Project_AddressName_Введите_правильный_адрес")]
		public string AddressName { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_Region_Регион")]
		[Required(ErrorMessageResourceType = typeof (LocalizationResource),
			ErrorMessageResourceName = "Project_Region_Введите_регион")]
		public string Region { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_Tags_Теги")]
		public List<Tag> Tags { get; set; }

		[Display(ResourceType = typeof(LocalizationResource), Name = "Project_Tags_Теги")]
		public Tag Tag {
			get { return Tags.FirstOrDefault(); }
			set
			{
				if (Tags == null)
				{
					Tags = new List<Tag>();
				}
				Tags[0] = value;
			}
		}


		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_InvestorUser_Инвестор")]
		public string InvestorUser { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_Tasks_Задачи")]
		public List<ProjectTask> Tasks { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_Steps_Шаги")]
		public List<Step> Steps { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_SubProject_Под_проекты")]
		public List<Project> SubProject { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_WorkflowState_Состояние")]
		public Workflow WorkflowState { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_IsInList_Находится_в_перечне_")]
		public bool IsInList { get; set; }

		[BsonIgnore]
		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_ProjectType_Тип_проекта")]
		public string ProjectType
		{
			get { return GetType().Name; }
		}

		[Display(ResourceType = typeof (LocalizationResource), Name = "Project_Info_Дополнительная_информация")]
		public List<AdditionalInfo> Info { get; set; }

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
		public List<ProjectNotes> ProjectNotes
		{
			get
			{
				return
					RepositoryContext.Current.All<ProjectNotes>(
						c => c.ProjectId == _id && !string.IsNullOrEmpty(c.NoteTitle)).ToList();
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

		[BsonRepresentation(BsonType.ObjectId)]
		public string _id { get; set; }
	}
}