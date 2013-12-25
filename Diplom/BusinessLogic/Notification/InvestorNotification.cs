using FluentEmail;
using Invest.Common;
using Invest.Common.Model.Project;
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
    }
}