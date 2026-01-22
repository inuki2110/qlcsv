using System.ComponentModel.DataAnnotations;

namespace QLCSV.Validation
{
    public class ValidUrlAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string url = value.ToString()!;
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("URL không hợp lệ (phải bắt đầu bằng http:// hoặc https://)");
        }
    }
}