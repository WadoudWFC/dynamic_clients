using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetCommercialCuttingQueryHandler : IRequestHandler<GetCommercialCuttingQuery, Result<IEnumerable<CommercialCuttingSanitized>>>
{
    private readonly IReferenceAglouDataService _referenceAglouDataService;
    private readonly ILogger<GetCommercialCuttingQueryHandler> _logger;
    public GetCommercialCuttingQueryHandler(IReferenceAglouDataService referenceAglouDataService, ILogger<GetCommercialCuttingQueryHandler> logger)
    {
        _referenceAglouDataService = referenceAglouDataService;
        _logger = logger;
    }
    public async Task<Result<IEnumerable<CommercialCuttingSanitized>>> Handle(GetCommercialCuttingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _referenceAglouDataService.GetCommercialCuttingAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[GetCommercialCutting]: {0} failed execution", nameof(GetCommercialCuttingQueryHandler));
                return Result<IEnumerable<CommercialCuttingSanitized>>.Failure(new Error("The GetCommercialCuttingQueryHandler failed", "Can't handle GetCommercialCuttingQueryHandler"));
            }
            _logger.LogInformation("[GetCommercialCutting]: Successful operation!");
            return Result<IEnumerable<CommercialCuttingSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[GetCommercialCutting]: {0}", ex.Message);
            return Result<IEnumerable<CommercialCuttingSanitized>>.Failure(new Error("The GetCommercialCuttingQueryHandler failed", ex.Message));
        }
    }
}
