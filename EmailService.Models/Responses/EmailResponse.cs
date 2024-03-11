namespace EmailService.Models.Responses
{
    public class EmailResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public EmailResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
            Message = SetMessage();
        }

        private string SetMessage()
        {
            return IsSuccess ? "Email was succesfully sent" : "Something went wrong";
        }
    }
}
