using System.Text.RegularExpressions;

namespace RedPrince.Services
{
    public static class PasswordValidator
    {
        private const int MinLength = 8;
        private const string UppercasePattern = @"[A-Z]";
        private const string LowercasePattern = @"[a-z]";
        private const string NumberPattern = @"[0-9]";
        private const string SpecialCharPattern = @"[!@#$%^&*()_+\-=\[\]{};':"",.<>?/\\|`~]";

        public static PasswordValidationResult ValidatePassword(string password)
        {
            var result = new PasswordValidationResult();

            if (string.IsNullOrWhiteSpace(password))
            {
                result.IsValid = false;
                result.Errors.Add("Password is required.");
                return result;
            }

            if (password.Length < MinLength)
            {
                result.IsValid = false;
                result.Errors.Add($"Password must be at least {MinLength} characters long.");
            }

            if (!Regex.IsMatch(password, UppercasePattern))
            {
                result.IsValid = false;
                result.Errors.Add("Password must contain at least one uppercase letter (A-Z).");
            }

            if (!Regex.IsMatch(password, LowercasePattern))
            {
                result.IsValid = false;
                result.Errors.Add("Password must contain at least one lowercase letter (a-z).");
            }

            if (!Regex.IsMatch(password, NumberPattern))
            {
                result.IsValid = false;
                result.Errors.Add("Password must contain at least one number (0-9).");
            }

            if (!Regex.IsMatch(password, SpecialCharPattern))
            {
                result.IsValid = false;
                result.Errors.Add("Password must contain at least one special character (!@#$%^&*()_+-=[]{}';:\",.<>?/\\|`~).");
            }

            if (result.Errors.Count == 0)
            {
                result.IsValid = true;
            }

            return result;
        }
    }

    public class PasswordValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
