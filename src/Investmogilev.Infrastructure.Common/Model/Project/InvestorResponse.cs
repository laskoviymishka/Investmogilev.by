// // -----------------------------------------------------------------------
// // <copyright file="InvestorResponse.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	#region Using

	using System;
	using System.ComponentModel.DataAnnotations;
	using Investmogilev.Infrastructure.Common.Localization;

	#endregion

	public class InvestorResponse
	{
		public string ProjectId { get; set; }

		public string ResponseId { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "InvestorResponse_ResponseDate_Дата_отклика")]
		public DateTime ResponseDate { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "InvestorResponse_InvestorFirstName_Имя")]
		public string InvestorFirstName { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "InvestorResponse_InvestorMiddleName_Отчество")]
		public string InvestorMiddleName { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "InvestorResponse_InvestorLastName_Фамилия")]
		public string InvestorLastName { get; set; }

		[Display(ResourceType = typeof (LocalizationResource),
			Name = "InvestorResponse_InvestorOrganizationName_Название_организации")]
		public string InvestorOrganizationName { get; set; }

		[Required]
		[Display(ResourceType = typeof (LocalizationResource),
			Name = "InvestorResponse_InvestorEmail_Адресс_электорнной_почты")]
		public string InvestorEmail { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "InvestorResponse_InvestorPhone_Телефон")]
		public string InvestorPhone { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "InvestorResponse_AdditionalInfo_Прочая_информация")]
		public string AdditionalInfo { get; set; }

		public string ExistingUser { get; set; }

		public bool IsVerified { get; set; }
	}
}