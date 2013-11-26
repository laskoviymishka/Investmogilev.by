using System;
using System.ComponentModel.DataAnnotations;
using Invest.Common.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model
{
    public class InvestorResponse
    {
        [Display(Name = "Дата отклика")]
        public string ResponseId { get; set; }


        [Display(Name = "Дата отклика")]
        public DateTime ResponseDate { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Введите имя")]
        public string InvestorFirstName { get; set; }

        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "Введите отчество")]
        public string InvestorMiddleName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Введите фамилию")]
        public string InvestorLastName { get; set; }

        [Display(Name = "Название организации")]
        [Required(ErrorMessage = "Введите название организации")]
        public string InvestorOrganizationName { get; set; }

        [Display(Name = "Адресс электорнной почты")]
        [Required(ErrorMessage = "Введите адресс электронной почти")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail введен не правильно ")]
        public string InvestorEmail { get; set; }

        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Введите телефон")]
        public string InvestorPhone { get; set; }

        [Display(Name = "Прочая информация")]
        public string AdditionalInfo { get; set; }

        public string ResponsedProjectId { get; set; }

        public bool IsVerified { get; set; }
    }
}