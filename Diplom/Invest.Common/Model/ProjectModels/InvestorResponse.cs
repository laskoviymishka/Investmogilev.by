using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.ProjectModels
{
    public class InvestorResponse
    {
        [Display(Name = "Дата отклика")]
        public string ResponseId { get; set; }


        [Display(Name = "Дата отклика")]
        public DateTime ResponseDate { get; set; }

        [Display(Name = "Имя")]
        public string InvestorFirstName { get; set; }

        [Display(Name = "Отчество")]
        public string InvestorMiddleName { get; set; }

        [Display(Name = "Фамилия")]
        public string InvestorLastName { get; set; }

        [Display(Name = "Название организации")]
        public string InvestorOrganizationName { get; set; }

        [Display(Name = "Адресс электорнной почты")]
        public string InvestorEmail { get; set; }

        [Display(Name = "Телефон")]
        public string InvestorPhone { get; set; }

        [Display(Name = "Прочая информация")]
        public string AdditionalInfo { get; set; }

        [Display(Name = "Уже существующий инвестор")]
        public string ExistingUser { get; set; }

        public string ResponsedProjectId { get; set; }

        public bool IsVerified { get; set; }
    }
}