using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class GetAllDossierQueryHandler : IRequestHandler<GetAllDossierQuery, Result<IEnumerable<DossierAllSanitized>>>
    {
        private readonly IDossierAglouService _dossierAglouService;
        private readonly ILogger<GetAllDossierQueryHandler> _logger;
        public GetAllDossierQueryHandler(IDossierAglouService dossierAglouService, ILogger<GetAllDossierQueryHandler> logger)
        {
            _dossierAglouService = dossierAglouService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<DossierAllSanitized>>> Handle(GetAllDossierQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierAglouService.GetAllDossierAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[GetAllDossier]: {0} failed execution!", nameof(GetAllDossierQueryHandler));
                    return Result<IEnumerable<DossierAllSanitized>>.Failure(new Error("The GetAllDossierQueryHandler failed", "Can't handle Get all dossier"));
                }
                _logger.LogInformation("[GetAllDossier]: Successful operation!");
                return Result<IEnumerable<DossierAllSanitized>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[GetAllDossier]: {0}", ex.Message);
                return Result<IEnumerable<DossierAllSanitized>>.Failure(new Error("The GetAllDossierQueryHandler failed", ex.Message));
            }
        }
    }
}
