namespace MultipleHttpClient.Application.Dossier.Queries
{
    public class HistorySearchRequest
    {
        public string? Field { get; set; } = "date_created";
        public string? Order { get; set; } = "desc";
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}
