using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using BusinessLogic.Providers;
using Invest.Common.Repository;
using WebMatrix.WebData;

namespace BusinessLogic.Notification
{
    class BaseNotificate
    {
        protected readonly IRepository Repository;
        protected readonly RoleProvider RoleProvider;
        protected readonly ExtendedMembershipProvider Membership;
        protected readonly SmtpClient Client;
        protected const string PassToViews = @"C:\Users\Andrei_Tserakhau\Documents\GitHub\DiplomMap\Diplom\BusinessLogic\Views\Emails\{0}.cshtml";

        protected BaseNotificate(IRepository repository)
        {
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
            RoleProvider = new MongoRoleProvider();
        }

        protected static string GetTemplate(string mailName)
        {
            using (var sr = new StreamReader(string.Format(BaseNotificate.PassToViews, mailName)))
            {
                return sr.ReadToEnd();
            }
        }
    }
}