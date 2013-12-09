using System;
using System.ComponentModel.DataAnnotations;

namespace Invest.Common.Model.ProjectModels
{
    public class BrownField : Project
    {
        [Display(Name = "Дата регистрации проекта")]
        public System.DateTime RegistrationDate { get; set; }

        [Display(Name = "Объем инвестиций")]
        public double InvestAmount { get; set; }

        [Display(Name = "Количество создоваемых рабочих мест")]
        public int WorkPlaces { get; set; }

        [Display(Name = "Примерное дата окончания проекта")]
        public System.DateTime EstimateRelease { get; set; }

        [Display(Name = "Прочая информация")]
        public string Appendix { get; set; }
    }
}
