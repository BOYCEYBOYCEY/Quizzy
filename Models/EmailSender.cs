using System.Net.Mail;
using System.Net;

namespace Quizzy_Main.Models
{
    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("quizzyapp7@gmail.com", "nunetahwtfeytopt")
            };

            MailMessage mailMessage = new MailMessage(from: "quizzyapp7@gmail.com", to:email, subject, message);
            mailMessage.IsBodyHtml = true;

            return client.SendMailAsync(mailMessage);
        }
    }
}
