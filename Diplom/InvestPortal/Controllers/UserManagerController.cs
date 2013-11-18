using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Invest.Common.Model;
using InvestPortal.Models;
using MongoRepository.Repository;
using Telerik.Web.Mvc;

namespace InvestPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {

        private readonly IRepository<MongoUser> _repository;

        public UserManagerController()
        {
            _repository = new BaseRepository<MongoUser>("mongodb://tserakhau.cloudapp.net", "Projects", "Users");
        }

        public ActionResult Index()
        {
            IList<MongoUser> users = _repository.GetAll();
            IList<UserManagerViewModel> model = new List<UserManagerViewModel>();
            foreach (MongoUser mongoUser in users)
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
            return View(new GridModel(_repository.GetAll()));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _SaveAjaxEditing(string id)
        {
            MongoUser product = _repository.GetById(id);
            UserManagerViewModel model = new UserManagerViewModel();
            UpdateUserModel(model, product);

            _repository.Update(product);

            return View(new GridModel(_repository.GetAll()));
        }

        private void UpdateUserModel(UserManagerViewModel model, MongoUser product)
        {
            UpdateModel(model);
            //TryUpdateModel(product);
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
            MongoUser product = new MongoUser();
            if (TryUpdateModel(product))
            {
                _repository.Insert(product);
            }
            return View(new GridModel(_repository.GetAll()));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _DeleteAjaxEditing(string id)
        {
            //Find a customer with ProductID equal to the id action parameter
            if (_repository.GetById(id) != null)
            {
                //Delete the record
                _repository.Delete(_repository.GetById(id));
            }

            //Rebind the grid
            return View(new GridModel(_repository.GetAll()));
        }
    }
}
