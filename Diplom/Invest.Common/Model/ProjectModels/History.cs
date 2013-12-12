using System;
using System.ComponentModel.DataAnnotations;
using Invest.Common.State;

namespace Invest.Common.Model.ProjectModels
{
    public class History
    {
        [Display(Name = "Кем изменено")]
        public string Editor { get; set; }

        [Display(Name = "Исходное состояние")]
        public ProjectStates FromState { get; set; }

        [Display(Name = "Конечное состояние")]
        public ProjectStates ToState { get; set; }

        [Display(Name = "Дата изменений")]
        public DateTime EditingTime { get; set; }
    }
}
