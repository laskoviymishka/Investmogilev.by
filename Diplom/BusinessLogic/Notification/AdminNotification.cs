using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using BusinessLogic.Providers;
using FluentEmail;
using Invest.Common;
using Invest.Common.Model.Project;
using Invest.Common.Model.User;
using Invest.Common.Repository;
using WebMatrix.WebData;

namespace BusinessLogic.Notification
{
    class AdminNotification : BaseNotificate, IAdminNotification
    {

        public AdminNotification()
            : base(RepositoryContext.Current)
        {
        }

        public void NotificateFill(Project currentProject)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                var email = Email
                            .From("laskoviymishka@gmail.com")
                            .UsingClient(Client)
                            .To(user.Email)
                            .Subject("Проект заполнен и перешел в состояние НА КАРТЕ")
                            .UsingTemplate(GetTemplate("FillInfomrationAdminMail"), currentProject);
                email.Send();
            }
        }

        public void MapEntryNotificate()
        {
        }

        public void NotificateReOpen()
        {
        }

        public void InvestorApprovedNotificate(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                var email = Email
                            .From("laskoviymishka@gmail.com")
                            .UsingClient(Client)
                            .To(user.Email)
                            .Subject("Проект заполнен и перешел в состояние НА КАРТЕ")
                            .UsingTemplate(GetTemplate("InvestorApprovedMail"), project);
                email.Send();
            }
        }

        public void InvestorResponsed(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                var email = Email
                            .From("laskoviymishka@gmail.com")
                            .UsingClient(Client)
                            .To(user.Email)
                            .Subject("Проект заполнен и перешел в состояние НА КАРТЕ")
                            .UsingTemplate(GetTemplate("InvestorResponsedMail"), project);
                email.Send();
            }
        }
    }
}