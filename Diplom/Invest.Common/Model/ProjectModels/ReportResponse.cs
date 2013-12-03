using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.ProjectModels
{
    public class ReportResponse : IMongoEntity
    {
        public string _id { get; set; }
        public string TaskId { get; set; }
        public string ReportId { get; set; }

        [Required]
        [Display(Name = "Автор")]
        public string ResponseUser { get; set; }

        [Required]
        [Display(Name = "Текст отзыва")]
        public string ResponseBody { get; set; }
        public IEnumerable<AdditionalInfo> Appendix { get; set; }

        [Required]
        [Display(Name = "Время создания отзыва")]
        public DateTime ResponseTime { get; set; }

        [Required]
        [Display(Name = "Одобрен?")]
        public bool IsApproved { get; set; }

        [Required]
        [Display(Name = "Отвергнут?")]
        public bool IsReject { get; set; }
    }
}
