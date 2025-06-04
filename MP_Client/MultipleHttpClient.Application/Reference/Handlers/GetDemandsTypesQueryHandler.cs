using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetDemandsTypesQueryHandler : IRequestHandler<GetDemandTypesQuery, Result<IEnumerable<DemandTypeSanitized>>>
{
    private readonly IReferenceAglouDataService _referenceAglouDataService;
    private readonly ILogger<GetDemandsTypesQueryHandler> _logger;
    public GetDemandsTypesQueryHandler(IReferenceAglouDataService referenceAglouDataService, ILogger<GetDemandsTypesQueryHandler> logger)
    {
        _referenceAglouDataService = referenceAglouDataService;
        _logger = logger;
    }
    public async Task<Result<IEnumerable<DemandTypeSanitized>>> Handle(GetDemandTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _referenceAglouDataService.GetDemandsTypeAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[GetDemandsType]: {0} failed execution", nameof(GetDemandsTypesQueryHandler));
                return Result<IEnumerable<DemandTypeSanitized>>.Failure(new Error("The GetDemandsTypesQueryHandler failed", "Can't handle GetDemandsTypesQueryHandler"));
            }
            _logger.LogInformation("[GetDemandsType]: Successful operation!");
            return Result<IEnumerable<DemandTypeSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[GetCommercialCutting]: {0}", ex.Message);
            return Result<IEnumerable<DemandTypeSanitized>>.Failure(new Error("The GetDemandsTypesQueryHandler failed", ex.Message));
        }
    }
}
