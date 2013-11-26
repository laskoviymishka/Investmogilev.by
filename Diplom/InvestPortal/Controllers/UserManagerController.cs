using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Invest.Common.Model;
using Invest.Common.Repository;
using Invest.Workflow.Project;
using InvestPortal.Models;
using Telerik.Web.Mvc;
using MongoRepository;

namespace InvestPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {

        private readonly IRepository _repository;

        public UserManagerController()
        {
            _repository = RepositoryContext.Current;
        }

        #region Assignee User

        public ActionResult UnUsignedProject()
        {
            ViewBag.Users = new List<BaseProjectController.NestedUserViewModel>();
            ViewBag.Regions = new List<BaseProjectController.NestedRegionViewModel>();
            foreach (Users mongoUser in RepositoryContext.Current.All<Users>())
            {
                ViewBag.Users.Add(new BaseProjectController.NestedUserViewModel() { Name = mongoUser.Username });
            }

            foreach (Region region in RepositoryContext.Current.All<Region>())
            {
                ViewBag.Regions.Add(new BaseProjectController.NestedRegionViewModel() { RegionName = region.RegionName });
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
            if (ModelState.IsValid)
            {
                var project = RepositoryContext.Current.GetOne<Project>(p => p._id == model._id);
                project.AssignUser = model.AssignUser;
                RepositoryContext.Current.Update(project);
                return RedirectToAction("UnUsignedProject");
            }
            else
            {
                return View(model);
            }
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
                var project = RepositoryContext.Current.GetOne<Project>(p => p.InvestorUser == user.Username);
                if (project != null)
                {
                    project.InvestorUser = "";

                    var response = project.Responses.FirstOrDefault(r => r.InvestorEmail.ToLower() == user.LoweredEmail);

                    if (response != null)
                    {
                        project.Responses.Remove(response);
                    }

                    string fromState = project.WorkflowState.CurrenState;
                    var workflow = project.WorkflowState;
                    workflow.CurrenState = GreenFieldStates.Open;
                    project.WorkflowState.ChangeHistory.Add(
                        new History()
                        {
                            EditingTime = DateTime.Now,
                            Editor = User.Identity.Name,
                            FromState = fromState,
                            ToState = GreenFieldStates.Open
                        });

                    RepositoryContext.Current.Update(project);
                }
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
                            Roles.AddUserToRole(mongoUser.Username,UserRoles.User.ToString());
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
