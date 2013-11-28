using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.User
{
    public class MessageBody
    {
        [Required]
        [Display(Name = "Сообщение")]
        public string Text { get; set; }
        public List<AdditionalInfo> Appendix { get; set; }
    }
}
