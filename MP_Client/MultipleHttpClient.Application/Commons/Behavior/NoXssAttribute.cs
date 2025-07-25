using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace MultipleHttpClient.Application;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class NoXssAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string stringValue)
        {
            var sanitizer = validationContext.GetService<IInputSanitizationService>();
            if (sanitizer?.ContainsMaliciousContent(stringValue) == true)
            {
                return new ValidationResult("Input contains potentially malicious content");
            }
        }
        return ValidationResult.Success;
    }
}
