using System.Web.Mvc;
using Investmogilev.Infrastructure.BusinessLogic.Notification;
using Investmogilev.Infrastructure.Common.Model.Common;

namespace Investmogilev.UI.Portal.Controllers
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