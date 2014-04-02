// // -----------------------------------------------------------------------
// // <copyright file="InvestorResponseViewModel.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Models
{
	#region Using

	using System;
	using System.ComponentModel.DataAnnotations;

	#endregion

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