using System.Web.Mvc;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Common;
using MongoDB.Bson;

namespace Investmogilev.UI.Portal.Controllers
{
    public class MailTemplateController : Controller
    {
        public ActionResult Index()
        {
            return View(RepositoryContext.Current.All<MailTemplate>());
        }

        //
        // GET: /MailTemplate/Details/5

        public ActionResult Details(string id)
        {
            return View(RepositoryContext.Current.GetOne<MailTemplate>(t => t._id == id));
        }

        //
        // GET: /MailTemplate/Create

        public ActionResult Create()
        {
            return View(new MailTemplate { _id = ObjectId.GenerateNewId().ToString() });
        }

        //
        // POST: /MailTemplate/Create

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(MailTemplate mailTemplate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RepositoryContext.Current.Add(mailTemplate);
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(mailTemplate);
            }
        }

        //
        // GET: /MailTemplate/Edit/5

        public ActionResult Edit(string id)
        {
            return View(RepositoryContext.Current.GetOne<MailTemplate>(t => t._id == id));
        }

        //
        // POST: /MailTemplate/Edit/5

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(MailTemplate mailTemplate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RepositoryContext.Current.Update(mailTemplate);
                    return RedirectToAction("Index");
                }

                return View(mailTemplate);
            }
            catch
            {
                return View(mailTemplate);
            }
        }

        public ActionResult Delete(string id)
        {
            RepositoryContext.Current.Delete<MailTemplate>(t => t._id == id);
            return RedirectToAction("Index");
        }
    }
}
