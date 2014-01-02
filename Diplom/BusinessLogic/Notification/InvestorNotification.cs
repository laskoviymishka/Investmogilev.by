using FluentEmail;
using Invest.Common;
using Invest.Common.Model.Project;
using System;
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
                    var email = Email
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
                var email = Email
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
            var email = Email
                       .From("laskoviymishka@gmail.com")
                       .UsingClient(Client)
                       .To(project.Responses[0].InvestorEmail)
                       .Subject("Обновления состояния ваших документов")
                       .UsingTemplate(GetTemplate("DocumentUpdate"), project)
                       .Send();
        }

        public void ProjectAproved(Project project)
        {
            var pass = Guid.NewGuid().ToString().Substring(0, 5);
            var login = project.Responses.Find(i => i.IsVerified).InvestorEmail;
            Membership.CreateAccount(login, pass);
            var email = Email
               .From("laskoviymishka@gmail.com")
               .UsingClient(Client)
               .To(project.Responses[0].InvestorEmail)
               .Subject("Ваша заявка одобрена")
               .UsingTemplate(GetTemplate("ProjectAproved"), new { Pass = pass, Login = login, Project = project })
               .Send();
        }
    }
}