using System;
using System.Net;
using System.Net.Mail;
using Invest.Common.Model;

namespace Invest.Common.Notification
{
    public class MailNotification : INotification
    {
        private static void SendMail(string from, string mailto, string caption, string message, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                var smtpClient = new SmtpClient()
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = true,
                        Host = "smtp.yandex.ru",
                        Port = 587,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("shahterv@yandex.ru", "negjqgfkflby1")
                    };
                smtpClient.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

        public bool NotificateUser(string from, string to, string title, string body)
        {
            SendMail("shahterv@yandex.ru", to, title, body);
            return true;
        }
    }
}