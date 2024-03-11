using EmailService.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EmailService.Models.Requests
{
    public class EmailRequest
    {
        [Required]
        public string? From { get; set; }
        [Required]
        [EmailAddressList]
        public List<string>? To { get; set; }
        [Required]
        public string? SenderName { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? HtmlPart { get; set; }
    }
}
