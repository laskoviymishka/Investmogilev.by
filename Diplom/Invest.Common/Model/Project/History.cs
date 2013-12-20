﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Invest.Common.Model.Project
{
    public class History
    {
        [Display(Name = "Кем изменено")]
        public string Editor { get; set; }

        [Display(Name = "Дата изменений")]
        public DateTime EditingTime { get; set; }
    }
}