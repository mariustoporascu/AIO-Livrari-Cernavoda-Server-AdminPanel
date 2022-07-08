using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;

namespace LivroManage.UI.Utilities
{

    public class EmailSender
    {
        private string emailUser;
        private string emailPass;

        public EmailSender(IConfiguration config)
        {
            emailUser = config["ConnectionStrings:EmailUser"];
            emailPass = config["ConnectionStrings:EmailPass"];
        }
        public bool SendEmail(string userEmail, string subject, string bodyText)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailUser);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = bodyText;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(emailUser, emailPass);
            client.Host = "mail5002.smarterasp.net";
            client.Port = 587;
            client.EnableSsl = true;
            client.Timeout = 30000;
            client.UseDefaultCredentials = false;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

    }
}
