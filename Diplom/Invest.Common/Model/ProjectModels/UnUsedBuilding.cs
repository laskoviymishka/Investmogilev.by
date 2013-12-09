﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Invest.Common.Model.ProjectModels
{
    public class UnUsedBuilding : Project
    {
        [Display(Name = "Площадь здания проекта")]
        public double Area { get; set; }

        [Display(Name = "На продажу?")]
        public bool IsSell { get; set; }

        [Display(Name = "Подведены комуникации?")]
        public bool IsCommunicate { get; set; }

        [Display(Name = "Балансовая стоимость здания")]
        public Nullable<double> BalancePrice { get; set; }
    }
}
