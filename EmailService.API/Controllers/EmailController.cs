using EmailService.Models.Requests;
using EmailService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail(EmailRequest request)
        {
            var result = await _emailService.SendEmail(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("SendBulkEmails")]
        public async Task<IActionResult> SendBulkEmails(List<EmailRequest> request)
        {
            var result = await _emailService.SendBulkEmails(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("SendEmailWithAttachment")]
        public async Task<IActionResult> SendEmailWithAttachment(EmailRequest request)
        {
            var result = await _emailService.SendEmailWithAttachment(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
