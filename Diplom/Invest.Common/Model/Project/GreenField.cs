using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
    [BsonIgnoreExtraElements]
    public class GreenField : Project
    {
        [Display(Name = "Инвестиционный номер")]
        [Required(ErrorMessage = "Введите инвестиционный номер")]
        public string InvestNumber { get; set; }

        [Display(Name = "Примерная дата инвестирования")]
        public System.DateTime EstimateInvestDate { get; set; }

        [Display(Name = "Цель проекта")]
        [Required(ErrorMessage = "Введите цель проекта")]
        public string Goal { get; set; }

        [Display(Name = "Площадь проекта")]
        [Required(ErrorMessage = "Введите площадь проекта")]
        public double Area { get; set; }

        [Display(Name = "Кадастровая стоимость проекта")]
        [Required(ErrorMessage = "Введите кадастровую стоимость проекта")]
        public double CadastrValue { get; set; }

        [Display(Name = "Прочие заинтересованные лица проекта")]
        public string ThirdPartiePretender { get; set; }

        [Display(Name = "Имя проекта")]
        [Required(ErrorMessage = "Введите имя проекта")]
        public string Infrastructure { get; set; }

        [Display(Name = "Владелец(город)")]
        public string OwnerCity { get; set; }

        [Display(Name = "Владелец(организация)")]
        public string Owner { get; set; }

        public Nullable<int> InvestRequest { get; set; }

        [Display(Name = "Прочая информация проекта")]
        public string Appendix { get; set; }
    }
}
