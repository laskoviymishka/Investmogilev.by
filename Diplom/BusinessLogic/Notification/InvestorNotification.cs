using FluentEmail;
using Invest.Common;
using Invest.Common.Model.Project;
using System;
using System.Web.Security;
namespace BusinessLogic.Notification
{
    public class InvestorNotification : BaseNotificate, IInvestorNotification
    {
        public InvestorNotification()
            : base(RepositoryContext.Current)
        {
        }

        public void InvestorResponsed(Project currentProject)
        {
            if (currentProject.Responses.Count > 1)
            {
                foreach (var response in currentProject.Responses)
                {
                    Email
                        .From("laskoviymishka@gmail.com")
                        .UsingClient(Client)
                        .To(response.InvestorEmail)
                        .Subject("Ваш отклик на проект с несколькими заявками (Инвестор)")
                        .UsingTemplate(GetTemplate("AdditionalResponseToInvestor"), currentProject)
                        .Send();
                }
            }
            else
            {
                Email
                    .From("laskoviymishka@gmail.com")
                    .UsingClient(Client)
                    .To(currentProject.Responses[0].InvestorEmail)
                    .Subject("Ваш отклик на проект (Инвестор)")
                    .UsingTemplate(GetTemplate("FirstResponseToInvestor"), currentProject)
                    .Send();
            }
        }

        public void DocumentUpdate(Project project)
        {
            Email
                .From("laskoviymishka@gmail.com")
                .UsingClient(Client)
                .To(project.Responses[0].InvestorEmail)
                .Subject("Обновления состояния документов по проекту " + project.Name)
                .UsingTemplate(GetTemplate("DocumentUpdate"), project)
                .Send();
        }

        public void ProjectAproved(Project project)
        {
            var pass = Guid.NewGuid().ToString().Substring(0, 5);
            var login = project.Responses.Find(i => i.IsVerified).InvestorEmail;
            Membership.CreateAccount(login, pass);
            Roles.AddUserToRole(login, "Investor");

            Email
                .From("laskoviymishka@gmail.com")
                .UsingClient(Client)
                .To(project.Responses[0].InvestorEmail)
                .Subject("Ваша заявка одобрена")
                .UsingTemplate(GetTemplate("ProjectAproved"), new { Pass = pass, Login = login, Project = project })
                .Send();
        }


        public void InvolvedOrganizationUpdate(Project project)
        {
            Email
                .From("laskoviymishka@gmail.com")
                .UsingClient(Client)
                .To(project.Responses[0].InvestorEmail)
                .Subject("Обновления состояния причастных лиц по проекту " + project.Name)
                .UsingTemplate(GetTemplate("InvolvedOrganizationUpdate"), project)
                .Send();
        }


        public void Comission(Comission comission, Project project)
        {
            Email
                .From("laskoviymishka@gmail.com")
                .UsingClient(Client)
                .To(project.Responses[0].InvestorEmail)
                .Subject("Назначена комиссия по проекту " + project.Name)
                .UsingTemplate(GetTemplate("Comission"), new { Project = project, Comission = comission })
                .Send();
        }


        public void WaitComission(Project project)
        {
            Email
                .From("laskoviymishka@gmail.com")
                .UsingClient(Client)
                .To(project.Responses[0].InvestorEmail)
                .Subject("Ожидается комиссия по проекту " + project.Name)
                .UsingTemplate(GetTemplate("WaitComission"), project)
                .Send();
        }


        public void UpdateComissionFix(Project project)
        {
            Email
                .From("laskoviymishka@gmail.com")
                .UsingClient(Client)
                .To(project.Responses[0].InvestorEmail)
                .Subject("Обновления состояния дороботок после комиссии " + project.Name)
                .UsingTemplate(GetTemplate("UpdateComissionFix"), project)
                .Send();
        }

        public void ComissionFixNeeded(Project project)
        {
            Email
                 .From("laskoviymishka@gmail.com")
                 .UsingClient(Client)
                 .To(project.Responses[0].InvestorEmail)
                 .Subject("Комиссия одобрила проект с дороботками " + project.Name)
                 .UsingTemplate(GetTemplate("ComissionFixNeeded"), project)
                 .Send();
        }


        public void WaitIspolcom(Project project)
        {
            Email
                 .From("laskoviymishka@gmail.com")
                 .UsingClient(Client)
                 .To(project.Responses[0].InvestorEmail)
                 .Subject("Проект направлен на исполком " + project.Name)
                 .UsingTemplate(GetTemplate("WaitIspolcom"), project)
                 .Send();
        }


        public void OnIspolcom(Comission comission, Project project)
        {
            Email
                 .From("laskoviymishka@gmail.com")
                 .UsingClient(Client)
                 .To(project.Responses[0].InvestorEmail)
                 .Subject("Назначен исполком по проекту " + project.Name)
                 .UsingTemplate(GetTemplate("OnIspolcom"), new { Project = project, Comission = comission })
                 .Send();
        }
    }
}