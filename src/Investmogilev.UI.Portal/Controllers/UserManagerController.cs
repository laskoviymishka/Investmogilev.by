﻿// // -----------------------------------------------------------------------
// // <copyright file="UserManagerController.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal.Controllers
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using System.Web.Security;
	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.User;
	using Investmogilev.Infrastructure.Common.Repository;
	using Investmogilev.UI.Portal.Models;

	#endregion

	[Authorize(Roles = "Admin")]
	public class UserManagerController : Controller
	{
		#region Private Fields

		private readonly IRepository _repository;

		#endregion

		#region Constructor

		public UserManagerController()
		{
			_repository = RepositoryContext.Current;
		}

		#endregion

		#region Create Update User

		public ActionResult CreateUser()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateUser(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				Membership.CreateUser(model.UserName, model.Password, model.Email);
				return RedirectToAction("Index", "Home");
			}
			return View(model);
		}

		public ActionResult Edit(string id)
		{
			Users user = RepositoryContext.Current.GetOne<Users>(u => u.Username == id);
			var model = new EditUserViewModel
			{
				UserName = id,
				Roles = Roles.GetRolesForUser(id).ToList(),
				NotificationTypeList = user.NotificationTypeList
			};
			return View(model);
		}

		[HttpPost]
		public ActionResult Edit(EditUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				string[] roles = Roles.GetRolesForUser(model.UserName);
				if (roles.Any(
					role => !model.Roles.Contains(role)))
				{
					Roles.RemoveUserFromRoles(
						model.UserName,
						roles.Where(
							role => !model.Roles.Contains(role)).ToArray());
				}
				if (model.Roles.Any(
					role => !roles.Contains(role)))
				{
					Roles.AddUserToRoles(
						model.UserName,
						model.Roles.Where(
							role => !roles.Contains(role)).ToArray());
				}
				Users user = RepositoryContext.Current.GetOne<Users>(u => u.Username == model.UserName);
				user.NotificationTypeList = model.NotificationTypeList;
				RepositoryContext.Current.Update(user);
				return RedirectToAction("Index", "UserManager");
			}
			return View(model);
		}

		#endregion

		#region Common user model

		public ActionResult Index()
		{
			return View(UserManagerViewModels);
		}

		#endregion

		#region Private helpers

		private IList<UserManagerViewModel> UserManagerViewModels
		{
			get
			{
				IList<Users> users = _repository.All<Users>().ToList();
				IList<UserManagerViewModel> model = new List<UserManagerViewModel>();
				foreach (var mongoUser in users)
				{
					var vm = new UserManagerViewModel
					{
						_id = mongoUser._id,
						Username = mongoUser.Username,
						LoweredEmail = mongoUser.Email,
						CreationDate = mongoUser.CreationDate,
						IsLockedOut = mongoUser.IsLockedOut,
						Password = "not showing",
						PasswordQuestion = mongoUser.PasswordQuestion,
						PasswordAnswer = mongoUser.PasswordAnswer,
						IsApproved = mongoUser.IsApproved
					};

					vm.Roles = new List<UserRoles>();
					string[] userRoles = Roles.GetRolesForUser(vm.Username);
					if (userRoles.Length > 0)
					{
						foreach (var role in userRoles)
						{
							UserRoles roleEnum;
							Enum.TryParse(role, true, out roleEnum);
							vm.Roles.Add(roleEnum);
						}
					}
					else
					{
						Roles.AddUserToRole(mongoUser.Username, UserRoles.User.ToString());
						vm.Roles.Add(UserRoles.User);
					}

					model.Add(vm);
				}
				return model;
			}
		}

		#endregion
	}
}