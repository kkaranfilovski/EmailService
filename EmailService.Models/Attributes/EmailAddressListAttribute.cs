using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EmailService.Models.Attributes
{
    public class EmailAddressListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not List<string> emailAddresses || !emailAddresses.Any())
            {
                return new ValidationResult("The email list is empty.");
            }

            var invalidEmails = emailAddresses.Where(email => !IsValidEmail(email)).ToList();

            if (invalidEmails.Any())
            {
                return new ValidationResult($"The following email addresses are invalid: {string.Join(", ", invalidEmails)}");
            }

            return ValidationResult.Success;
        }

        private bool IsValidEmail(string email)
        {
            // Regex pattern for email validation
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
