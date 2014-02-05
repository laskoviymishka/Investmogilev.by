﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Investmogilev.UI.Portal.Models
{
    public class SubTaskViewModel
    {
        [Required]
        [Display(Name = "Название контрольной точки")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Описание контрольной точки")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Ожидаемая дата прохождения контрольной точки")]
        public DateTime Milestone { get; set; }
        public string ParentId { get; set; }
        public string ProjectId { get; set; }
    }
}