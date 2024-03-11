using EmailService.Models.Dtos;
using EmailService.Models.Requests;
using EmailService.Models.Responses;
using EmailService.Services.Interfaces;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Serilog;

namespace EmailService.Services.Services
{
    public class MailJetEmailService : IEmailService
    {
        private readonly IMailjetClient _mailjetClient;

        public MailJetEmailService(IMailjetClient mailjetClient)
        {
            _mailjetClient = mailjetClient;
        }

        public async Task<EmailResponse> SendEmail(EmailRequest request)
        {
            try
            {
                var recepients = request.To?.Select(x => new SendContact(x));
                
                var email = new TransactionalEmailBuilder()
                            .WithFrom(new SendContact(request.From, request.SenderName))
                            .WithTo(recepients)
                            .WithSubject(request.Subject)
                            .WithHtmlPart(request.HtmlPart)
                            .Build();

                var response = await _mailjetClient.SendTransactionalEmailAsync(email, isSandboxMode: true);
                var isSuccess = response.Messages != null && response.Messages.First().Status == "success";
                
                if (isSuccess)
                {
                    Log.Information("Email sent succesfully to: {@recepients}", response?.Messages?.SelectMany(x => x.To.Select(x => x.Email)));
                }
                else
                {
                    Log.Error("Error sending email: {@errors}", response.Messages);
                }

                return new EmailResponse(isSuccess);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Sending email failed");
                return new EmailResponse(false);
            }
        }

        public async Task<EmailResponse> SendBulkEmails(List<EmailRequest> request)
        {
            try
            {
                var emails = new List<TransactionalEmail>();
                foreach (var emailRequest in request)
                {
                    var recepients = emailRequest.To?.Select(x => new SendContact(x));

                    var email = new TransactionalEmailBuilder()
                                .WithFrom(new SendContact(emailRequest.From, emailRequest.SenderName))
                                .WithTo(recepients)
                                .WithSubject(emailRequest.Subject)
                                .WithHtmlPart(emailRequest.HtmlPart)
                                .Build();

                    emails.Add(email);
                }

                var response = await _mailjetClient.SendTransactionalEmailsAsync(emails, isSandboxMode: true);
                var isSuccess = response?.Messages?.Any(x => x.Status == "success") ?? false;

                if (isSuccess)
                {
                    Log.Information("Email sent succesfully to: {@recepients}", response?.Messages?.SelectMany(x => x.To.Select(x => x.Email)));
                    
                    var areUnsuccesfullySent = response?.Messages?.Any(x => x.Status != "success") ?? false;
                    if (areUnsuccesfullySent)
                    {
                        Log.Error("Email was NOT sent succesfully to: {@response}", response?.Messages?.Where(x => x.Status != "success"));
                    }
                }
                else
                {
                    Log.Error("Error sending email: {@errors}", response?.Messages);
                }

                return new EmailResponse(isSuccess);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Sending email failed");
                return new EmailResponse(false);
            }
        }

        public async Task<EmailResponse> SendEmailWithAttachment(EmailRequest request)
        {
            try
            {
                var attachment = GetImgFileFromDisk();

                var recepients = request.To?.Select(x => new SendContact(x));

                var email = new TransactionalEmailBuilder()
                            .WithFrom(new SendContact(request.From, request.SenderName))
                            .WithTo(recepients)
                            .WithSubject(request.Subject)
                            .WithHtmlPart(request.HtmlPart)
                            .WithAttachment(new Attachment(attachment.Filename, attachment.ContentType, attachment.Base64Content))
                            .Build();

                var response = await _mailjetClient.SendTransactionalEmailAsync(email, isSandboxMode: true);
                var isSuccess = response.Messages != null && response.Messages.First().Status == "success";

                if (isSuccess)
                {
                    Log.Information("Email sent succesfully to: {@recepients}", response?.Messages?.SelectMany(x => x.To.Select(x => x.Email)));
                }
                else
                {
                    Log.Error("Error sending email: {@errors}", response?.Messages);
                }

                return new EmailResponse(isSuccess);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Sending email failed");
                return new EmailResponse(false);
            }
        }

        /// <summary>
        /// Used to get image from disk, for testing purposes
        /// </summary>
        /// <returns>image from disk</returns>
        private static AttachmentDto GetImgFileFromDisk()
        {
            var path = "../EmailService.Services/Resources/attachment.jpg";
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileBytes = File.ReadAllBytes(path);
            var base64String = Convert.ToBase64String(fileBytes);

            return new AttachmentDto
            {
                Filename = fileName,
                ContentType = "image/jpg",
                Base64Content = base64String,
            };
        }
    }
}
