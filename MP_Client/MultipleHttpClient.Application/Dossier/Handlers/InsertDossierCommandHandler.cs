using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Command;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Handlers
{
    public class InsertDossierCommandHandler : IRequestHandler<InsertDossierCommand, Result<InsertDossierOperationResult>>
    {
        private readonly IDossierAglouService _dossierAglouService;
        private readonly ILogger<InsertDossierCommandHandler> _logger;
        public InsertDossierCommandHandler(IDossierAglouService dossierAglouService, ILogger<InsertDossierCommandHandler> logger)
        {
            _dossierAglouService = dossierAglouService;
            _logger = logger;
        }

        public async Task<Result<InsertDossierOperationResult>> Handle(InsertDossierCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dossierAglouService.InsertDossierAsync(request);
                if (!result.IsSuccess || result.Value == null)
                {
                    _logger.LogError("[InsertDossier]: {0} failed execution!", nameof(InsertDossierCommandHandler));
                    return Result<InsertDossierOperationResult>.Failure(new Error("The InsertDossierCommandHandler failed", "Can't handle insert dossier"));
                }
                _logger.LogInformation("[InsertDossier]: Successful operation!");
                return Result<InsertDossierOperationResult>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("[InsertDossier]: {0}", ex.Message);
                return Result<InsertDossierOperationResult>.Failure(new Error("The InsertDossierCommandHandler failed", "Can't handle insert dossier"));
            }
        }
    }
}
