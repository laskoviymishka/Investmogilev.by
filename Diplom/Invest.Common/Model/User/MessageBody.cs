using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Model.Common;

namespace Investmogilev.Infrastructure.Common.Model.User
{
    public class MessageBody
    {
        [Required]
        [Display(Name = "Сообщение")]
        public string Text { get; set; }
        public List<AdditionalInfo> Appendix { get; set; }
    }
}
