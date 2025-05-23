namespace MultipleHtppClient.Infrastructure.HTTP.Configurations;

public sealed record Configuration
{
    public string DefaultApiName { get; set; } = string.Empty;
    public List<ApiConfig> Apis { get; set; } = new List<ApiConfig>();
}
