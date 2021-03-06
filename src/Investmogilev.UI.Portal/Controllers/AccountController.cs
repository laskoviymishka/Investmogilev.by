﻿// // -----------------------------------------------------------------------
// // <copyright file="AccountController.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Controllers
{
	#region Using

	using System.Linq;
	using System.Web.Mvc;
	using System.Web.Security;
	using Investmogilev.UI.Portal.Models;
	using WebMatrix.WebData;

	#endregion

	[Authorize]
	public class AccountController : Controller
	{
		//
		// GET: /Account/Login

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
		}

		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("All", "BaseProject");
			}
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login

		[AllowAnonymous]
		[HttpPost]
		public ActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
			{
				return RedirectToAction("All", "BaseProject");
			}

			ModelState.AddModelError("", "The user name or password provided is incorrect.");
			return PartialView(model);
		}

		[AllowAnonymous]
		public ActionResult UserToRole(string userName, string roleName)
		{
			if (!Roles.GetAllRoles().Contains(roleName))
			{
				Roles.CreateRole(roleName);
			}

			if (Membership.FindUsersByName(userName) != null)
			{
				Roles.AddUserToRole(userName, roleName);
			}

			return RedirectToAction("Index", "Home");
		}

		//
		// POST: /Account/LogOff`

		public ActionResult LogOff()
		{
			WebSecurity.Logout();

			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /Account/Register

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				try
				{
					WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
					WebSecurity.Login(model.UserName, model.Password);
					return RedirectToAction("Index", "Home");
				}
				catch (MembershipCreateUserException e)
				{
					ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "Users name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return
						"The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return
						"The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return
						"An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
	}
}