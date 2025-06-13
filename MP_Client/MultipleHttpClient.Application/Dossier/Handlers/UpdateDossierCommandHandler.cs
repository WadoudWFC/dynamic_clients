using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Command;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class UpdateDossierCommandHandler : IRequestHandler<UpdateDossierCommand, Result<DossierUpdateResult>>
    {
        private readonly IDossierAglouService _dossierAglouService;
        private readonly ILogger<UpdateDossierCommandHandler> _logger;
        public UpdateDossierCommandHandler(IDossierAglouService dossierAglouService, ILogger<UpdateDossierCommandHandler> logger)
        {
            _dossierAglouService = dossierAglouService;
            _logger = logger;
        }

        public async Task<Result<DossierUpdateResult>> Handle(UpdateDossierCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierAglouService.UpdateDossierAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[UpdateDossier]: {0} failed execution!", nameof(UpdateDossierCommandHandler));
                    return Result<DossierUpdateResult>.Failure(new Error("The UpdateDossierCommandHandler failed", "Can't handle Update dossier"));
                }
                _logger.LogInformation("[UpdateDossier]: Successful operation!");
                return Result<DossierUpdateResult>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[UpdateDossier]: {0}", ex.Message);
                return Result<DossierUpdateResult>.Failure(new Error("The UpdateDossierCommandHandler failed", ex.Message));
            }
        }
    }
}
