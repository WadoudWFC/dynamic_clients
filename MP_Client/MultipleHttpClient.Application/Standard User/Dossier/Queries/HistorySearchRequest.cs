namespace MultipleHttpClient.Application.Dossier.Queries
{
    public record HistorySearchRequest
    {
        public string? Field { get; init; }
        public string? Order { get; init; }
        public int? Skip { get; init; }
        public int? Take { get; init; }
    }
}
