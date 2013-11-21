using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Invest.Common.Model;
using Invest.Common.Repository;
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

        public ActionResult Index()
        {
            IList<Users> users = _repository.All<Users>().ToList();
            IList<UserManagerViewModel> model = new List<UserManagerViewModel>();
            foreach (Users mongoUser in users)
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
                int i = 0;
                vm.Roles = new List<UserRoles>();
                foreach (string role in Roles.GetRolesForUser(vm.Username))
                {
                    UserRoles roleEnum = new UserRoles();
                    UserRoles.TryParse(role, true, out roleEnum);
                    vm.Roles.Add(roleEnum);
                }
                model.Add(vm);
            }
            return View(model);
        }

        [GridAction]
        public ActionResult _SelectAjaxEditing()
        {
            return View(new GridModel(_repository.All<Users>()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _SaveAjaxEditing(string id)
        {
            Users product = _repository.GetOne<Users>(u => u._id == id);
            UserManagerViewModel model = new UserManagerViewModel();
            UpdateUserModel(model, product);

            _repository.Update(product);

            return View(new GridModel(_repository.All<Users>()));
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
            Users product = new Users();
            if (TryUpdateModel(product))
            {
                _repository.Add(product);
            }
            return View(new GridModel(_repository.All<Users>()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteAjaxEditing(string id)
        {
            if (_repository.GetOne<Users>(u => u._id == id) != null)
            {
                _repository.Delete(_repository.GetOne<Users>(u => u._id == id));
            }

            return View(new GridModel(_repository.All<Users>()));
        }
    }
}
