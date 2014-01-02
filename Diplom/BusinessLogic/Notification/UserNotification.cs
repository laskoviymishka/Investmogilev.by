﻿using System.Web.Security;
using FluentEmail;
using Invest.Common;
using Invest.Common.Model.Project;

namespace BusinessLogic.Notification
{
    class UserNotification :BaseNotificate, IUserNotification
    {
        public UserNotification()
            :base(RepositoryContext.Current)
        {
        }

        public void NotificateOpen(Project currentProject)
        {
            var users = RoleProvider.GetUsersInRole("User");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                var email = Email
                            .From("laskoviymishka@gmail.com")
                            .UsingClient(Client)
                            .To(user.Email)
                            .Subject("Проект открыт (Пользователь)")
                            .UsingTemplate(GetTemplate("ProjectOpen"), currentProject);
                email.Send();
            }
        }

        public void InvestorResponsed(Project currentProject)
        {
            var users = RoleProvider.GetUsersInRole("User");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                var email = Email
                            .From("laskoviymishka@gmail.com")
                            .UsingClient(Client)
                            .To(user.Email)
                            .Subject("На проект получен отклик (Пользователь)")
                            .UsingTemplate(GetTemplate("InvestorResponded"), currentProject);
                email.Send();
            }
        }
    }
}