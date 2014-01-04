using System.Web.Mvc;
using BusinessLogic.Notification;
using Invest.Common.Model.Common;

namespace InvestPortal.Controllers
{
    public class ProfileController : Controller
    {
        private readonly PortalNotificationHub _notificationHub;

        public ProfileController()
        {
            _notificationHub = new PortalNotificationHub();
        }

        public ActionResult ProfilePartial()
        {
            if (User.Identity.IsAuthenticated)
            {
                return PartialView(new PortalNotification());
            }

            return PartialView();
        }

        public ActionResult MenuPartial()
        {
            return PartialView();
        }
    }
}