using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Command;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class InsertDossierCommandHandler : IRequestHandler<InsertDossierCommand, Result<InsertDossierOperationResult>>
    {
        private readonly IDossierAglouService _dossierService;
        private readonly ILogger<InsertDossierCommandHandler> _logger;

        public InsertDossierCommandHandler(IDossierAglouService dossierService, ILogger<InsertDossierCommandHandler> logger)
        {
            _dossierService = dossierService;
            _logger = logger;
        }

        public async Task<Result<InsertDossierOperationResult>> Handle(InsertDossierCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierService.InsertDossierAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[InsertDossier]: Failed to create dossier for user {UserId}", request.UserId);
                    return Result<InsertDossierOperationResult>.Failure(new Error("InsertDossierFailed", "Unable to create dossier"));
                }

                _logger.LogInformation("[InsertDossier]: Successfully created dossier {DossierId} for user {UserId}",
                    result.Value.DossierId, request.UserId);
                return Result<InsertDossierOperationResult>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[InsertDossier]: Exception creating dossier for user {UserId}", request.UserId);
                return Result<InsertDossierOperationResult>.Failure(new Error("InsertDossierFailed", ex.Message));
            }
        }
    }
}
