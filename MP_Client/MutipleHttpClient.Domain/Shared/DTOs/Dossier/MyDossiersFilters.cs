namespace MutipleHttpClient.Domain.Shared.DTOs.Dossier
{
    public record MyDossiersFilters(int? Skip, int? Take, string? OrderBy, string? Order);
}
