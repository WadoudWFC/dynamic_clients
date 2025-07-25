using System.Text.RegularExpressions;
using FluentValidation;

namespace MultipleHttpClient.Application;

public static class XssValidationExtensions
{
    public static IRuleBuilder<T, string> NoXss<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(NotContainMaliciousContent).WithMessage("Input contains potentially malicious content");
    }

    public static IRuleBuilder<T, string> NoHtmlTags<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(NotContainHtmlTags).WithMessage("Input cannot contain HTML tags");
    }
    public static IRuleBuilder<T, string> NoScriptTags<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(NotContainScriptTags).WithMessage("Input contains invalid script content");
    }
    private static bool NotContainMaliciousContent(string input)
    {
        if (string.IsNullOrEmpty(input)) return true;

        var dangerous = new[]
        {
            "javascript:", "vbscript:", "onload=", "onerror=", "onclick=",
            "onmouseover=", "<script", "</script>", "eval(", "expression("
        };

        return !dangerous.Any(pattern => input.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
    private static bool NotContainHtmlTags(string input)
    {
        if (string.IsNullOrEmpty(input)) return true;
        return !Regex.IsMatch(input, @"<[^>]*>", RegexOptions.IgnoreCase);
    }
    private static bool NotContainScriptTags(string input)
    {
        if (string.IsNullOrEmpty(input)) return true;
        var dangerous = new[] { "<script", "javascript:", "vbscript:", "onload=", "onerror=" };
        return !dangerous.Any(d => input.Contains(d, StringComparison.OrdinalIgnoreCase));
    }
}
