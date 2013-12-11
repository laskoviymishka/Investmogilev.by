using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvestPortal.Models
{
    public class ExistingResponseViewModel
    {
        public string ProjectId { get; set; }
        public string ResponseId { get; set; }
        public DateTime ResponseTime { get; set; }

        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли должны совпадать.")]
        public string ConfirmPassword { get; set; }
    }
}