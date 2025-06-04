using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetArrondissementQueryHandler : IRequestHandler<GetArrondissementQuery, Result<IEnumerable<ArrondissementSanitized>>>
{
    private readonly IReferenceAglouDataService _referenceAglouDataService;
    private readonly ILogger<GetArrondissementQueryHandler> _logger;
    public GetArrondissementQueryHandler(IReferenceAglouDataService referenceAglouDataService, ILogger<GetArrondissementQueryHandler> logger)
    {
        _referenceAglouDataService = referenceAglouDataService;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<ArrondissementSanitized>>> Handle(GetArrondissementQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _referenceAglouDataService.GetArrondissementsAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[GetArrondissements]: {0} failed execution", nameof(GetArrondissementQueryHandler));
                return Result<IEnumerable<ArrondissementSanitized>>.Failure(new Error("The GetArrondissementQueryHandler failed", "Can't handle GetArrondissementQueryHandler"));
            }
            _logger.LogInformation("[GetArrondissements]: Successful operation!");
            return Result<IEnumerable<ArrondissementSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[GetArrondissements]: {0}", ex.Message);
            return Result<IEnumerable<ArrondissementSanitized>>.Failure(new Error("The GetArrondissementQueryHandler failed", ex.Message));
        }
    }
}
