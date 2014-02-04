using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.User;
using Investmogilev.Infrastructure.Common.Repository;
using Investmogilev.UI.Portal.Models;

namespace Investmogilev.UI.Portal.Controllers
{
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
			var model = new EditUserViewModel
				{
					UserName = id,
					Roles = Roles.GetRolesForUser(id).ToList()
				};
			return View(model);
		}

		[HttpPost]
		public ActionResult Edit(EditUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				var roles = Roles.GetRolesForUser(model.UserName);
				if(roles.Any(
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
					var userRoles = Roles.GetRolesForUser(vm.Username);
					if (userRoles.Length > 0)
					{
						foreach (string role in userRoles)
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