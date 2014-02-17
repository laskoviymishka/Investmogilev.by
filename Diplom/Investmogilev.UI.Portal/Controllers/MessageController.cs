using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Investmogilev.Infrastructure.BusinessLogic.Managers;
using Investmogilev.Infrastructure.BusinessLogic.Notification;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.User;
using Investmogilev.Infrastructure.Common.Repository;
using MongoDB.Bson;

namespace Investmogilev.UI.Portal.Controllers
{
	[Authorize]
	public class MessageController : Controller
	{
		#region Private Fields

		private const string Template = "messageAppendix/{1}/{0}/";

		private readonly PortalMessageHandler _portalMessage;
		private readonly IRepository _userRepository;

		#endregion

		#region Constructor

		public MessageController()
		{
			_portalMessage = new PortalMessageHandler();
			_userRepository = new MongoRepository(WebConfigurationManager.AppSettings["mongoServer"],
				WebConfigurationManager.AppSettings["mongoBase"]);
		}

		#endregion

		#region Main Methods

		public ActionResult Index()
		{
			return View(_portalMessage.Inbox(User.Identity.Name));
		}

		public ActionResult Draft()
		{
			return View(_portalMessage.Draft(User.Identity.Name));
		}

		public ActionResult Unreaded()
		{
			return View(_portalMessage.Unread(User.Identity.Name));
		}

		public ActionResult Sent()
		{
			return View(_portalMessage.Sended(User.Identity.Name));
		}

		public ActionResult UnreadedSent()
		{
			return View(_portalMessage.SendedUnread(User.Identity.Name));
		}

		public ActionResult Message(string id)
		{
			return View(_portalMessage.Message(User.Identity.Name, id));
		}

		public ActionResult ReadMessage(string id)
		{
			MessageQueue message = _portalMessage.ReadMessage(User.Identity.Name, id);
			if (message != null)
			{
				return View(message);
			}

			return RedirectToAction("Index");
		}

		public ActionResult EditMessage(string id)
		{
			MessageQueue message = _portalMessage.Message(User.Identity.Name, id);
			if (message != null)
			{
				if (!message.IsSended)
				{
					return View(message);
				}

				return RedirectToAction("Message", new {id = message.Id});
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult EditMessage(MessageQueue model)
		{
			if (ModelState.IsValid)
			{
				_portalMessage.PushMessage(model);
				return RedirectToAction("Draft");
			}

			return View(model);
		}


		public ActionResult NewMessage()
		{
			BindUsers();
			return View(new MessageQueue {Id = ObjectId.GenerateNewId().ToString(), From = User.Identity.Name});
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult NewMessage(MessageQueue model)
		{
			if (ModelState.IsValid)
			{
				if (_userRepository.GetOne<Users>(u => u.LoweredUsername == model.To.ToLower()) != null)
				{
					model.IsSended = true;
					_portalMessage.PushMessage(model);
					return RedirectToAction("Draft");
				}

				ModelState.AddModelError("To", "Пользователь не найден");
			}
			BindUsers();
			return View(model);
		}

		#endregion

		#region Appendix

		public ActionResult Save(string id, IEnumerable<HttpPostedFileBase> attachments)
		{
			foreach (HttpPostedFileBase file in attachments)
			{
				string fileName = Path.GetFileName(file.FileName);
				string physicalPath =
					AdditionalInfoManager.GetPhysicalPath(
						Template,
						new[]
						{
							User.Identity.Name,
							DateTime.Now.ToString("MM-dd-yy")
						});

				file.SaveAs(physicalPath + fileName);

				_portalMessage.AddAppendix(
					User.Identity.Name,
					id,
					new DocumentAdditionalInfo
					{
						FilePath = physicalPath + fileName,
						InfoName = fileName,
						Id = ObjectId.GenerateNewId().ToString()
					});
			}

			return Content("");
		}

		public FileResult Download(string messageId, string appendixId)
		{
			var doc = _portalMessage.Appendix(User.Identity.Name, messageId, appendixId) as DocumentAdditionalInfo;
			return doc != null ? File(doc.FilePath, "application/doc", doc.InfoName) : null;
		}

		#endregion

		#region Private Helpers

		private void BindUsers()
		{
			List<string> users = _userRepository.All<Users>().Select(mongoUser => mongoUser.Username).ToList();
			ViewBag.Users = users;
		}

		#endregion
	}
}