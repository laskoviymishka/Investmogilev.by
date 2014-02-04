using System.Net;
using System.Net.Mail;
using FluentEmail;

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
    public class MailMessageHandler
    {
        private readonly SmtpClient client;

        public MailMessageHandler()
        {
            client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            NetworkCredential myCreds = new NetworkCredential("laskoviymishka@gmail.com", "p0iuytrewq", "");
            client.Credentials = myCreds;
        }

        public void Send()
        {
            var template = "Dear @Model.Name, You are totally @Model.Compliment.";

            this.Send("test", "laskoviymishka@gmail.com", template);
        }

        private void Send(string subject, string to, string template)
        {
            var email = Email
                .FromDefault()
                .To(to)
                .Subject(subject)
                .UsingTemplate(template, new { Name = "Luke", Compliment = "Awesome" })
                .UsingClient(client);

            email.Send();
        }

        public void SendNewTaskNotification()
        {
        
        }

        public void SendNewProjectNotification()
        {
        
        }
    }
}
