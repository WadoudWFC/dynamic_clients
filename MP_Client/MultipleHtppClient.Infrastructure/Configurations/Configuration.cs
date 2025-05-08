namespace MultipleHtppClient.Infrastructure;

public sealed record Configuration
{
    public string DefaultApiName { get; set; } = string.Empty;
    public List<ApiConfig> Apis { get; set; } = new List<ApiConfig>();
}
