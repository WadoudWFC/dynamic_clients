using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class GetDossierStatusQueryHandler : IRequestHandler<GetDossierStatusQuery, Result<IEnumerable<DossierStatusSanitized>>>
    {
        private readonly IDossierAglouService _dossierAglouService;
        private readonly ILogger<GetDossierStatusQueryHandler> _logger;
        public GetDossierStatusQueryHandler(IDossierAglouService dossierAglouService, ILogger<GetDossierStatusQueryHandler> logger)
        {
            _dossierAglouService = dossierAglouService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<DossierStatusSanitized>>> Handle(GetDossierStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierAglouService.GetDossierStatusAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[GetDossierStatus]: {0} failed execution", nameof(GetDossierStatusQueryHandler));
                    return Result<IEnumerable<DossierStatusSanitized>>.Failure(new Error("The GetDossierStatusQueryHandler failed", "Can't handle get dossier status!"));
                }
                _logger.LogInformation("[GetDossierStatus]: Successful operation!");
                return Result<IEnumerable<DossierStatusSanitized>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[GetDossierStatus]: {0}", ex.Message);
                return Result<IEnumerable<DossierStatusSanitized>>.Failure(new Error("The GetDossierStatusQueryHandler failed", ex.Message));
            }
        }
    }
}
