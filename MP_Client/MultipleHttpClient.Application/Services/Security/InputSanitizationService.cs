using System.Text.RegularExpressions;
using System.Web;

namespace MultipleHttpClient.Application;

public class InputSanitizationService : IInputSanitizationService
{
    private static readonly Regex ScriptPattern = new Regex(
            @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex HtmlPattern = new Regex(
        @"<[^>]*>", RegexOptions.Compiled);
    private static readonly string[] DangerousPatterns = {
        "javascript:", "vbscript:", "onload=", "onerror=", "onclick=",
        "onmouseover=", "<script", "</script>", "eval(", "expression("
    };
    public string SanitizeHtml(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return HttpUtility.HtmlEncode(input);
    }
    public string SanitizeForJson(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var sanitized = HtmlPattern.Replace(input, "");
        return HttpUtility.HtmlEncode(sanitized);
    }
    public bool ContainsMaliciousContent(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        var lowerInput = input.ToLowerInvariant();
        return DangerousPatterns.Any(pattern => lowerInput.Contains(pattern.ToLowerInvariant()));
    }
}
