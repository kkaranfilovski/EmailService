namespace EmailService.Models.Dtos
{
    public class AttachmentDto
    {
        public string? Filename { get; set; }

        public string? ContentType { get; set; }

        public string? Base64Content { get; set; }
    }
}
