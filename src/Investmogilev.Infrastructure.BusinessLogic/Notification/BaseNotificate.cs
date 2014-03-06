using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using FluentEmail;
using Investmogilev.Infrastructure.BusinessLogic.Providers;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Model.User;
using Investmogilev.Infrastructure.Common.Repository;
using Investmogilev.Infrastructure.Common.State;
using WebMatrix.WebData;

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	public class BaseNotificate
	{
		protected const string PassToViews = "~/App_Data/MailTemplate/{0}.cshtml";
		protected readonly SmtpClient Client;
		protected readonly ExtendedMembershipProvider Membership;
		protected readonly IRepository Repository;
		protected readonly RoleProvider RoleProvider;
		private readonly PortalNotificationHub _protalNotification;

		protected BaseNotificate(IRepository repository)
		{
			RoleProvider = new MongoRoleProvider();
			Membership = new MongoMembership();
			var config = new NameValueCollection();
			config["applicationName"] = "InvestProject";
			config["connectionString"] = WebConfigurationManager.AppSettings["mongoServer"];
			config["database"] = WebConfigurationManager.AppSettings["mongoBase"];
			RoleProvider.Initialize("roles", config);
			Membership = new MongoMembership();
			Membership.Initialize("MongoMembership", config);
			_protalNotification = new PortalNotificationHub();
			Client = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				UseDefaultCredentials = false,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential("laskoviymishka@gmail.com", "p0iuytrewq")
			};
			Repository = repository;
		}

		protected static string GetTemplate(string mailName)
		{
			using (var sr = new StreamReader(string.Format(HttpContext.Current.Server.MapPath(PassToViews), mailName)))
			{
				return sr.ReadToEnd();
			}
		}

		protected void SendMailFromDb(Project project, dynamic model, ProjectWorkflow.Trigger trigger, UserType userType)
		{
			var template = Repository.GetOne<MailTemplate>(t => t.Trigger == trigger && t.UserType == userType);
			if (template == null)
			{
				template = new MailTemplate
				{
					UserType = userType,
					Trigger = trigger,
					Title = "Шаблон письма отсутствует",
					Body =
						string.Format("Необходимо добавить шаблон письма для {0} пользователя на {1} событие", userType,
							trigger)
				};
			}

			if (template.UserType == UserType.Admin)
			{
				string[] users = RoleProvider.GetUsersInRole("Admin");

				foreach (string userName in users)
				{
					MembershipUser user = Membership.GetUser(userName, false);
					if (user != null)
					{
						Email
							.From("laskoviymishka@gmail.com")
							.UsingClient(Client)
							.To(user.Email)
							.Subject(template.Title)
							.UsingTemplate(template.Body, model)
							.Send();
						_protalNotification.PushNotificate(new NotificationQueue
						{
							IsRead = false,
							NotificationTime = DateTime.Now,
							NotificationTitle = template.Title,
							NotigicationBody = template.Body,
							UserName = user.UserName
						});
					}
				}
			}

			if (template.UserType == UserType.Investor)
			{
				Email
					.From("laskoviymishka@gmail.com")
					.UsingClient(Client)
					.To(project.Responses[0].InvestorEmail)
					.Subject(template.Title)
					.UsingTemplate(template.Body, model)
					.Send();
				_protalNotification.PushNotificate(new NotificationQueue
				{
					IsRead = false,
					NotificationTime = DateTime.Now,
					NotificationTitle = template.Title,
					NotigicationBody = template.Body,
					UserName = project.Responses[0].InvestorEmail
				});
			}

			if (template.UserType == UserType.User)
			{
				string[] users = RoleProvider.GetUsersInRole("User");

				foreach (string userName in users)
				{
					MembershipUser user = Membership.GetUser(userName, false);
					if (user != null)
					{
						Email
							.From("laskoviymishka@gmail.com")
							.UsingClient(Client)
							.To(user.Email)
							.Subject(template.Title)
							.UsingTemplate(template.Body, model)
							.Send();
						_protalNotification.PushNotificate(new NotificationQueue
						{
							IsRead = false,
							NotificationTime = DateTime.Now,
							NotificationTitle = template.Title,
							NotigicationBody = template.Body,
							UserName = user.UserName
						});
					}
				}
			}
		}
	}
}