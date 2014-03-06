using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	[BsonIgnoreExtraElements]
	public class UnUsedBuilding : Project
	{
		[Display(Name = "Площадь здания проекта, кв. м.")]
		public double Area { get; set; }

		[Display(Name = "На продажу?")]
		public bool IsSell { get; set; }

		[Display(Name = "Подведены комуникации?")]
		public bool IsCommunicate { get; set; }

		[Display(Name = "Балансовая стоимость здания, млрд. руб.")]
		public double? BalancePrice { get; set; }
	}
}