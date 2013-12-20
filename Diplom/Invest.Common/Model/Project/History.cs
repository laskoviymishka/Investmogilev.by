using System;
using System.ComponentModel.DataAnnotations;
using Invest.Common.State;

namespace Invest.Common.Model.Project
{
    public class History
    {
        [Display(Name = "Кем изменено")]
        public string Editor { get; set; }

        [Display(Name = "Исходное состояние")]
        public ProjectWorkflow.State From { get; set; }

        [Display(Name = "Конечное состояние")]
        public ProjectWorkflow.State To { get; set; }

        [Display(Name = "Дополнительная информация")]
        public string Body { get; set; }

        [Display(Name = "Дата изменений")]
        public DateTime EditingTime { get; set; }
    }
}
