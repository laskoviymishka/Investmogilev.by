// // -----------------------------------------------------------------------
// // <copyright file="MailMessageHandler.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	#region Using

	using System.Net;
	using System.Net.Mail;
	using FluentEmail;

	#endregion

	public class MailMessageHandler
	{
		private readonly SmtpClient client;

		public MailMessageHandler()
		{
			client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			var myCreds = new NetworkCredential("laskoviymishka@gmail.com", "p0iuytrewq", "");
			client.Credentials = myCreds;
		}

		public void Send()
		{
			string template = "Dear @Model.Name, You are totally @Model.Compliment.";

			Send("test", "laskoviymishka@gmail.com", template);
		}

		private void Send(string subject, string to, string template)
		{
			Email email = Email
				.FromDefault()
				.To(to)
				.Subject(subject)
				.UsingTemplate(template, new {Name = "Luke", Compliment = "Awesome"})
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