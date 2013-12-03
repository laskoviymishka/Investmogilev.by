using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.ProjectModels
{
    public class TaskReport : IMongoEntity
    {
        public string _id { get; set; }

        [Required]
        [Display(Name = "Текст отчета")]
        public string Body { get; set; }
        public IEnumerable<AdditionalInfo> Appendix { get; set; }

        [Required]
        [Display(Name = "Одобрен?")]
        public bool IsApproved { get; set; }

        [Required]
        [Display(Name = "Время создания")]
        public DateTime ReportTime { get; set; }
        public string ProjectId { get; set; }

        [Required]
        [Display(Name = "Автор")]
        public string ReporterName { get; set; }

        public string TaskId { get; set; }

        public ReportResponse Response { get; set; }
    }
}
