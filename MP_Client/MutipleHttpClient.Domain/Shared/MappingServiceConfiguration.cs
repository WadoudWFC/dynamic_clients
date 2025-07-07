namespace MutipleHttpClient.Domain;

public class MappingServiceConfiguration
{
    public const string SectionName = "MappingService";
    public string ApplicationSalt { get; set; } = string.Empty;
    public bool EnableCache { get; set; } = true;
    public int CacheExpirationHours { get; set; } = 24;
    public int ReferenceCacheExpirationDays { get; set; } = 1;
    public bool UseDeterministic { get; set; } = true;
    public bool EnableMetrics { get; set; } = true;
    public int MaxCacheSize { get; set; } = 10000;
}
