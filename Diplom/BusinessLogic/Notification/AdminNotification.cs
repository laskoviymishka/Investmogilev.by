using System.Web.Security;
using FluentEmail;
using Invest.Common;
using Invest.Common.Model.Project;

namespace BusinessLogic.Notification
{
    public class AdminNotification : BaseNotificate, IAdminNotification
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

        public void WaitInvolved(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Необходимо заполнить причастных лиц для проекта " + project.Name)
                    .UsingTemplate(GetTemplate("WaitInvolved"), project)
                    .Send();
            }
        }


        public void InvolvedOrganizationUpdate(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Обновления состояния причастных лиц по проекту " + project.Name)
                    .UsingTemplate(GetTemplate("InvolvedOrganizationUpdate"), project)
                    .Send();
            }
        }


        public void Comission(Comission comission, Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Назначена комиссия по проекту " + project.Name)
                    .UsingTemplate(GetTemplate("Comission"), new { Project = project, Comission = comission })
                    .Send();
            }
        }


        public void WaitComission(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Ожидается комиссия по проекту " + project.Name)
                    .UsingTemplate(GetTemplate("WaitComission"), project)
                    .Send();
            }
        }


        public void UpdateComissionFix(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Обновления состояния исправлений после комиссии " + project.Name)
                    .UsingTemplate(GetTemplate("UpdateComissionFix"), project)
                    .Send();
            }
        }


        public void WaitIspolcom(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Проект ожидает на исполком " + project.Name)
                    .UsingTemplate(GetTemplate("WaitIspolcom"), project)
                    .Send();
            }
        }


        public void OnIspolcom(Comission comission, Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Проект направлен на исполком " + project.Name)
                    .UsingTemplate(GetTemplate("OnIspolcom"), new { Project = project, Comission = comission })
                    .Send();
            }
        }


        public void UpdateIspolcomFix(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Обновления состояния исправлений после исполкома " + project.Name)
                    .UsingTemplate(GetTemplate("UpdateIspolcomFix"), project)
                    .Send();
            }
        }


        public void PlanCreatingUpdate(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Обновления состояния дорожной карты проекта " + project.Name)
                    .UsingTemplate(GetTemplate("PlanCreatingUpdate"), project)
                    .Send();
            }
        }


        public void Realization(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Реализация проекта " + project.Name)
                    .UsingTemplate(GetTemplate("Realization"), project)
                    .Send();
            }
        }


        public void Done(Project project)
        {
            var users = RoleProvider.GetUsersInRole("Admin");

            foreach (string userName in users)
            {
                var user = Membership.GetUser(userName, false);
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(user.Email)
                    .Subject("Проект завершен " + project.Name)
                    .UsingTemplate(GetTemplate("Done"), project)
                    .Send();
            }
        }
    }
}