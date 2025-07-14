namespace MultipleHttpClient.Application.Dossier.Queries
{
    public record HistorySearchRequest
    {
        public string? Field { get; init; }
        public string? Order { get; init; }
        [NonNegative]
        public int? Skip { get; init; }
        [NonNegative]
        public int? Take { get; init; }
    }
}
