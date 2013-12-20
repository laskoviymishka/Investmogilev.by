using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common;
using Invest.Common.Model.Common;
using Invest.Common.Model.Project;
using Invest.Common.Model.User;
using Invest.Common.Repository;
using InvestPortal.Models;
using Telerik.Web.Mvc;

namespace InvestPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        #region Private Fields

        private readonly IRepository _repository;

        #endregion

        public UserManagerController()
        {
            _repository = RepositoryContext.Current;
        }


        #region Create User

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
                foreach (Users mongoUser in users)
                {
                    if (!Roles.IsUserInRole(mongoUser.Username, UserRoles.Admin.ToString()))
                    {
                        var vm = new UserManagerViewModel()
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
                }
                return model;
            }
        }

        #endregion
    }
}
