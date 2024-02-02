using Complaint_Report_Registering_API.Entities;
using MailKit.Net.Smtp;
using MimeKit;

namespace Complaint_Report_Registering_API.Services
{
    public class MailService : IMailService
    {
        public async Task<bool> SendMailAsync(MailData mailData)
        {
            try
            {
                using MimeMessage emailMessage = new MimeMessage();
                MailboxAddress emailFrom = new MailboxAddress("Sodara Sou", "sodarasou168@gmail.com");
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = mailData.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = mailData.EmailBody;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                using SmtpClient mailClient = new SmtpClient();
                await mailClient.ConnectAsync("sandbox.smtp.mailtrap.io", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await mailClient.AuthenticateAsync("33ec4913dc51fb", "36e4b3b23cf842");
                await mailClient.SendAsync(emailMessage);
                await mailClient.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                return false;
                // Rethrow the exception
                throw;
            }
        }
    }
}