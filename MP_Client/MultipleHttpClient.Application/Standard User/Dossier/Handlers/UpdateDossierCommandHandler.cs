using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Command;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class UpdateDossierCommandHandler : IRequestHandler<UpdateDossierCommand, Result<DossierUpdateResult>>
    {
        private readonly IDossierAglouService _dossierService;
        private readonly ILogger<UpdateDossierCommandHandler> _logger;

        public UpdateDossierCommandHandler(IDossierAglouService dossierService, ILogger<UpdateDossierCommandHandler> logger)
        {
            _dossierService = dossierService;
            _logger = logger;
        }

        public async Task<Result<DossierUpdateResult>> Handle(UpdateDossierCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierService.UpdateDossierAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[UpdateDossier]: Failed to update dossier {0}", request.DossierId);
                    return Result<DossierUpdateResult>.Failure(new Error("UpdateDossierFailed", "Unable to update dossier"));
                }

                _logger.LogInformation("[UpdateDossier]: Successfully updated dossier {0}", request.DossierId);
                return Result<DossierUpdateResult>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[UpdateDossier]: Exception updating dossier {0}", request.DossierId);
                return Result<DossierUpdateResult>.Failure(new Error("UpdateDossierFailed", ex.Message));
            }
        }
    }
}
