using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.ProjectModels
{
    public class InvestorResponse
    {
        public string ResponseId { get; set; }

        [Required]
        [Display(Name = "Дата отклика")]
        public DateTime ResponseDate { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string InvestorFirstName { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string InvestorMiddleName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string InvestorLastName { get; set; }

        [Required]
        [Display(Name = "Название организации")]
        public string InvestorOrganizationName { get; set; }

        [Required]
        [Display(Name = "Адресс электорнной почты")]
        public string InvestorEmail { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        public string InvestorPhone { get; set; }

        [Display(Name = "Прочая информация")]
        public string AdditionalInfo { get; set; }

        public string ExistingUser { get; set; }

        public string ResponsedProjectId { get; set; }

        public bool IsVerified { get; set; }
    }
}