using EmailService.Models.Requests;
using EmailService.Models.Responses;

namespace EmailService.Services.Interfaces
{
    public interface IEmailService
    {
        Task<EmailResponse> SendEmail(EmailRequest request);
        Task<EmailResponse> SendBulkEmails(List<EmailRequest> request);
        Task<EmailResponse> SendEmailWithAttachment (EmailRequest request);
    }
}
