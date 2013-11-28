using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic;
using Invest.Common.Model.Common;
using Invest.Common.Model.User;
using InvestPortal.Models;
using MongoDB.Bson;
using MongoRepository;

namespace InvestPortal.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        #region Private Fields

        private readonly PortalNotificationHandler _portalNotification;

        #endregion

        #region Constructor

        public MessageController()
        {
            _portalNotification = new PortalNotificationHandler();
        }

        #endregion

        public ActionResult Index()
        {
            return View(_portalNotification.Inbox(User.Identity.Name));
        }

        public ActionResult Draft()
        {
            return View(_portalNotification.Draft(User.Identity.Name));
        }

        public ActionResult Unreaded()
        {
            return View(_portalNotification.Unread(User.Identity.Name));
        }

        public ActionResult Sent()
        {
            return View(_portalNotification.Sended(User.Identity.Name));
        }

        public ActionResult UnreadedSent()
        {
            return View(_portalNotification.SendedUnread(User.Identity.Name));
        }

        public ActionResult Message(string id)
        {
            return View(_portalNotification.Message(User.Identity.Name, id));
        }

        public ActionResult ReadMessage(string id)
        {
            var message = _portalNotification.ReadMessage(User.Identity.Name, id);
            if (message != null)
            {
                return View(message);
            }

            return RedirectToAction("Index");
        }

        public ActionResult EditMessage(string id)
        {
            var message = _portalNotification.Message(User.Identity.Name, id);
            if (message != null)
            {
                if (!message.IsSended)
                {
                    return View(message);
                }

                return RedirectToAction("Message", new { id = message._id });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditMessage(MessageQueue model)
        {
            if (ModelState.IsValid)
            {
                _portalNotification.PushMessage(model);
                return RedirectToAction("Draft");
            }

            return View(model);
        }

        public ActionResult NewMessage()
        {
            BindUsers();
            return View(new MessageQueue() { _id = ObjectId.GenerateNewId().ToString(), From = User.Identity.Name });
        }


        [HttpPost]
        public ActionResult NewMessage(MessageQueue model)
        {
            if (ModelState.IsValid)
            {
                if (RepositoryContext.Current.GetOne<Users>(u => u.LoweredUsername == model.To.ToLower()) != null)
                {
                    _portalNotification.PushMessage(model);
                    return RedirectToAction("Draft");
                }

                ModelState.AddModelError("To", "Пользователь не найден");
            }
            BindUsers();
            return View(model);
        }

        #region Appendix

        public ActionResult Save(string id, IEnumerable<HttpPostedFileBase> attachments)
        {
            string physicalPath = "";
            string fileName = "";
            foreach (var file in attachments)
            {
                fileName = Path.GetFileName(file.FileName);
                physicalPath = Path.Combine(
                    Server.MapPath(string.Format("~/App_Data/messageAppendix/{1}/{0}/", User.Identity.Name, DateTime.Now.ToShortDateString())),
                    fileName);

                if (!Directory.Exists(
                        Server.MapPath(string.Format("~/App_Data/messageAppendix/{1}/{0}/", User.Identity.Name, DateTime.Now.ToShortDateString()))))
                {
                    Directory.CreateDirectory(Server.MapPath(string.Format("~/App_Data/messageAppendix/{1}/{0}/", User.Identity.Name, DateTime.Now.ToShortDateString())));
                }

                file.SaveAs(physicalPath);
                DocumentAdditionalInfo document = new DocumentAdditionalInfo();
                document.FilePath = physicalPath;
                document.InfoName = fileName;
                document._id = ObjectId.GenerateNewId().ToString();
                _portalNotification.AddAppendix(User.Identity.Name, id, document);
            }
            return Content("");
        }

        public FileResult Download(string messageId, string appendixId)
        {
            var doc = _portalNotification.Appendix(User.Identity.Name, messageId, appendixId) as DocumentAdditionalInfo;
            return File(doc.FilePath, "application/doc", doc.InfoName);
        }

        #endregion

        #region Private Helpers

        private void BindUsers()
        {
            List<string> users = new List<string>();
            foreach (var mongoUser in RepositoryContext.Current.All<Users>())
            {
                users.Add(mongoUser.Username);
            }
            ViewBag.Users = users;
        }

        #endregion
    }
}
