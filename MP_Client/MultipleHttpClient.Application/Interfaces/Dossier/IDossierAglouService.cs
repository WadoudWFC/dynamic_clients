using MultipleHttpClient.Application.Dossier.Command;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application;

public interface IDossierAglouService
{
    Task<Result<IEnumerable<HistorySearchSanitized>>> SearchHistroyAsync(SearchHistoryQuery query);
    Task<Result<IEnumerable<CommentSanitized>>> GetAllCommentsAsync(GetAllCommentsQuery query);
    Task<Result<IEnumerable<DossierStatusSanitized>>> GetDossierStatusAsync(GetDossierStatusQuery query);
    Task<Result<IEnumerable<DossierAllSanitized>>> GetAllDossierAsync(GetAllDossierQuery query);
    Task<Result<DossierCountsSanitized>> GetCountsAsync(GetCountsQuery query);
    Task<Result<CommentOperationResult>> InsertCommentAsync(InsertCommentCommand command);
    Task<Result<DossierUpdateResult>> UpdateDossierAsync(UpdateDossierCommand command);
    Task<Result<IEnumerable<DossierSearchSanitized>>> SearchDossierAsync(SearchDossierQuery query);
    Task<Result<InsertDossierOperationResult>> InsertDossierAsync(InsertDossierCommand command);
}
