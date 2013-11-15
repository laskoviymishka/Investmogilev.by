using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AdminPanelUI.Models;

namespace AdminPanelUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            var users = Membership.GetAllUsers();
            IList<UserAdmin> model = new List<UserAdmin>();
            foreach (MembershipUser user in users)
            {
                model.Add(new UserAdmin()
                    {
                        Mail = user.Email,
                        Name = user.UserName,
                        Roles = Roles.GetRolesForUser(user.UserName)
                    });
            }
            return View(model);
        }

    }
}
