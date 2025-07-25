namespace MultipleHttpClient.Application;

public interface IInputSanitizationService
{
    string SanitizeHtml(string input);
    string SanitizeForJson(string input);
    bool ContainsMaliciousContent(string input);
}
