using System;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Localization;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	[BsonIgnoreExtraElements]
	public class GreenField : Project
	{
		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_InvestNumber_Инвестиционный_номер")]
		public string InvestNumber { get; set; }

		[Display(ResourceType = typeof (LocalizationResource),
			Name = "GreenField_EstimateInvestDate_Примерная_дата_инвестирования")]
		public DateTime EstimateInvestDate { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_Goal_Цель_проекта")]
		public string Goal { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_Area_Площадь_проекта")]
		[Required(ErrorMessageResourceType = typeof (LocalizationResource),
			ErrorMessageResourceName = "GreenField_Area_Введите_площадь_проекта")]
		public double Area { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_CadastrValue_Кадастровая_стоимость_проекта")
		]
		public double CadastrValue { get; set; }

		[Display(ResourceType = typeof (LocalizationResource),
			Name = "GreenField_EstimateInvoiceAmount_Ожидаемая_сумма_инвестирования")]
		public double EstimateInvoiceAmount { get; set; }

		[Display(ResourceType = typeof (LocalizationResource),
			Name = "GreenField_ThirdPartiePretender_Прочие_заинтересованные_лица_проекта")]
		public string ThirdPartiePretender { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_Infrastructure_Имя_проекта")]
		public string Infrastructure { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_OwnerCity_Владелец_город_")]
		public string OwnerCity { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_Owner_Владелец_организация_")]
		public string Owner { get; set; }

		public int? InvestRequest { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_Appendix_Прочая_информация_проекта")]
		public string Appendix { get; set; }
	}
}