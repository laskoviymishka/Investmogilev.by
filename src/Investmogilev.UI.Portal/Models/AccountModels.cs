// // -----------------------------------------------------------------------
// // <copyright file="AccountModels.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Models
{
	#region Using

	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	#endregion

	public class LocalPasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class LoginModel
	{
		[Required]
		[Display(Name = "Имя пользователя")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Display(Name = "Запомнить меня?")]
		public bool RememberMe { get; set; }
	}

	public class RegisterModel
	{
		[Required]
		[Display(Name = "Имя пользователя")]
		public string UserName { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Повторите пароль")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email пользователя")]
		public string Email { get; set; }
	}

	public class InvestorRegisterModel
	{
		[Required]
		[Display(Name = "Имя пользователя (Login)")]
		public string UserName { get; set; }

		[Required]
		[Display(Name = "Email пользователя")]
		public string Email { get; set; }

		[Display(Name = "Пароль")]
		public string Password { get; set; }

		public string ResponseId { get; set; }

		public string ResponseProjectId { get; set; }
	}

	public class UserViewModel
	{
		[Required]
		[Display(Name = "Имя пользователя (Login)")]
		public string UserName { get; set; }
	}

	public class EditUserViewModel
	{
		[Required]
		[Display(Name = "Имя пользователя (Login)")]
		public string UserName { get; set; }

		[Display(Name = "Роли пользовтеля")]
		public List<string> Roles { get; set; }

		[Display(Name = "Уведомление о проектах типов")]
		public List<string> NotificationTypeList { get; set; }
	}
}