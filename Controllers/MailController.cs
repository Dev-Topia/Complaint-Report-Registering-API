using Complaint_Report_Registering_API.Entities;
using Complaint_Report_Registering_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Complaint_Report_Registering_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController(IMailService mailService) : ControllerBase
    {
        [HttpPost("send-mail")]
        public async Task<bool> SendMail(MailData mailData)
        {
            return await mailService.SendMailAsync(mailData);
        }
    }
}