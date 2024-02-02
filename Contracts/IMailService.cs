using Complaint_Report_Registering_API.Entities;

namespace Complaint_Report_Registering_API.Services
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(MailData mailData);
    }
}