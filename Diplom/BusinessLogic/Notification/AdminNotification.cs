using System.Web.Security;
using FluentEmail;
using Invest.Common;
using Invest.Common.Model.Project;

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
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Проект заполнен и перешел в состояние НА КАРТЕ (Администратор)")
                    .UsingTemplate(GetTemplate("FillInfomrationAdminMail"), currentProject)
                    .Send();
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
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Инвестор потвержден (Администратор)")
                    .UsingTemplate(GetTemplate("InvestorApprovedMail"), project)
                    .Send();
            }
        }

        public void InvestorResponsed(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Инвестор откликнулся (Администратор)")
                    .UsingTemplate(GetTemplate("InvestorResponsedMail"), project)
                    .Send();
            }
        }


        public void DocumentUpdate(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Обновления состояния документов по проекту " + project.Name)
                    .UsingTemplate(GetTemplate("DocumentUpdate"), project)
                    .Send();
            }
        }
    }
}