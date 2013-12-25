using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using BusinessLogic.Providers;
using Invest.Common.Repository;
using WebMatrix.WebData;
using System.Web;

namespace BusinessLogic.Notification
{
    public class BaseNotificate
    {
        protected readonly IRepository Repository;
        protected readonly RoleProvider RoleProvider;
        protected readonly ExtendedMembershipProvider Membership;
        protected readonly SmtpClient Client;
        protected const string PassToViews = "~/App_Data/MailTemplate/{0}.cshtml";

        protected BaseNotificate(IRepository repository)
        {
            RoleProvider = new MongoRoleProvider();
            Membership = new MongoMembership();
            var config = new NameValueCollection();
            config["applicationName"] = "InvestProject";
            config["connectionString"] = "mongodb://tserakhau.cloudapp.net";
            config["database"] = "Projects";
            RoleProvider.Initialize("roles", config);
            Membership = new MongoMembership();
            Membership.Initialize("MongoMembership", config);

            Client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("laskoviymishka@gmail.com", "p0iuytrewq")
            };
            Repository = repository;
        }

        protected static string GetTemplate(string mailName)
        {
            using (var sr = new StreamReader(string.Format(HttpContext.Current.Server.MapPath(PassToViews).ToString(), mailName)))
            {
                return sr.ReadToEnd();
            }
        }
    }
}