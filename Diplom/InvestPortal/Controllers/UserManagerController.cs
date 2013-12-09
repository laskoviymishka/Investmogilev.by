using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Managers;
using Invest.Common.Model.Common;
using Invest.Common.Model.ProjectModels;
using Invest.Common.Model.User;
using Invest.Common.Repository;
using InvestPortal.Models;
using MongoRepository;
using Telerik.Web.Mvc;

namespace InvestPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        #region Private Fields

        private readonly IRepository _repository;
        private readonly ProjectStateManager _stateManager;

        #endregion

        public UserManagerController()
        {
            _repository = RepositoryContext.Current;
            _stateManager = new ProjectStateManager();
        }

        #region Assignee User

        public ActionResult UnUsignedProject()
        {
            ViewBag.Users = new List<BaseProjectController.NestedUserViewModel>();
            foreach (Users mongoUser in RepositoryContext.Current.All<Users>())
            {
                ViewBag.Users.Add(new BaseProjectController.NestedUserViewModel() { Name = mongoUser.Username });
            }

            var project = RepositoryContext.Current.All<Project>(p => string.IsNullOrEmpty(p.AssignUser)).ToList();
            return View(project);
        }

        public ActionResult AssignUser(string id)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == id);

            if (project != null)
            {
                ViewBag.Users = new List<BaseProjectController.NestedUserViewModel>();
                foreach (Users mongoUser in RepositoryContext.Current.All<Users>())
                {
                    if (Roles.IsUserInRole(mongoUser.Username, "Admin") ||
                        Roles.IsUserInRole(mongoUser.Username, "User"))
                    {
                        ViewBag.Users.Add(new BaseProjectController.NestedUserViewModel() { Name = mongoUser.Username });
                    }
                }

                return View(project);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult AssignUser(Project model)
        {
            _stateManager.SetContext(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name));
            _stateManager.AssignProjectToUser(model._id, model.AssignUser);
            return RedirectToAction("WorkFlowForProject", "BaseProject", new { id = model._id });
        }

        #endregion

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

        #region Grid Action

        [GridAction]
        public ActionResult _SelectAjaxEditing()
        {
            return View(new GridModel(_repository.All<Users>()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _SaveAjaxEditing(string id)
        {
            var userManagerViewModel = UserManagerViewModels.FirstOrDefault(u => u._id == id);
            if (userManagerViewModel != null)
            {
                IList<UserRoles> roles = userManagerViewModel.Roles;
                var user = RepositoryContext.Current.GetOne<Users>(pr => pr._id == id);
                UserManagerViewModel model = new UserManagerViewModel();
                UpdateUserModel(model, user);
                if (model.Roles != roles)
                {
                    foreach (UserRoles userRolese in roles)
                    {
                        Roles.RemoveUserFromRole(model.Username, userRolese.ToString());
                    }

                    foreach (var userInRole in model.Roles)
                    {
                        Roles.AddUserToRole(model.Username, userInRole.ToString());
                    }
                }
                _repository.Update(user);
            }
            return View(new GridModel(UserManagerViewModels));
        }

        private void UpdateUserModel(UserManagerViewModel model, Users product)
        {
            UpdateModel(model);
            product.Username = model.Username;
            product.Email = model.LoweredEmail;
            product.CreationDate = model.CreationDate;
            product.IsLockedOut = model.IsLockedOut;
            product.PasswordQuestion = model.PasswordQuestion;
            product.PasswordAnswer = model.PasswordAnswer;
            product.IsApproved = model.IsApproved;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _InsertAjaxEditing()
        {
            return View(new GridModel(_repository.All<Users>()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteAjaxEditing(string id)
        {
            var user = _repository.GetOne<Users>(u => u._id == id);
            if (user != null)
            {
                //_stateManager.SetContext(User.Identity.Name,Roles.GetRolesForUser(User.Identity.Name));
                //_stateManager.RemoveAssignee(user.Username, user.Email, User.Identity.Name);
                Membership.DeleteUser(user.Username);
                Roles.RemoveUserFromRoles(user.Username, Roles.GetRolesForUser(user.Username));
            }

            return View(new GridModel(UserManagerViewModels));
        }

        #endregion

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
