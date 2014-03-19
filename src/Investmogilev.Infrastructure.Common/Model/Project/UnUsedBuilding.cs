using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	[BsonIgnoreExtraElements]
	public class UnUsedBuilding : Project
	{
		[Display(Name = "Площадь здания, кв. м.")]
		public double AreaBuilding { get; set; }

		[Display(Name = "Площадь земли, кв. м.")]
		public double Area { get; set; }

		[Display(Name = "Аренда")]
		public bool IsRent { get; set; }

		[Display(Name = "Отчуждение")]
		public bool IsExclusion { get; set; }

		[Display(Name = "Стоимость здания, млрд. руб.")]
		public double? BalancePrice { get; set; }
	}
}