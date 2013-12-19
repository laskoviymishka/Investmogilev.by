using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Invest.Common;
using Invest.Common.Model.User;

namespace InvestPortal.Controllers
{
    public class NotificationController : Controller
    {
        public ActionResult Index()
        {
            return View(RepositoryContext.Current.All<NotificationQueue>(q => q.UserName == User.Identity.Name && !q.IsRead).OrderBy(t => t.NotificationTime));
        }

        public ActionResult All()
        {
            return View(RepositoryContext.Current.All<NotificationQueue>(q => q.UserName == User.Identity.Name));
        }
    }
}
