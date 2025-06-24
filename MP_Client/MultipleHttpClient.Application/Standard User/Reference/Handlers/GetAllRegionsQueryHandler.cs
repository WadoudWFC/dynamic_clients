using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetAllRegionsQueryHandler : IRequestHandler<GetAllRegionQuery, Result<IEnumerable<RegionsSanitized>>>
{
    private readonly IReferenceAglouDataService _referenceAglouDataService;
    private readonly ILogger<GetAllRegionsQueryHandler> _logger;
    public GetAllRegionsQueryHandler(IReferenceAglouDataService referenceAglouDataService, ILogger<GetAllRegionsQueryHandler> logger)
    {
        _referenceAglouDataService = referenceAglouDataService;
        _logger = logger;
    }
    public async Task<Result<IEnumerable<RegionsSanitized>>> Handle(GetAllRegionQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _referenceAglouDataService.GetAllRegionsAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[GetAllRegions]: {0} failed execution", nameof(GetAllRegionsQueryHandler));
                return Result<IEnumerable<RegionsSanitized>>.Failure(new Error("The GetAllRegionsQueryHandler failed", "Can't handle GetAllRegionsQueryHandler"));
            }
            _logger.LogInformation("[GetAllRegions]: Successful operation!");
            return Result<IEnumerable<RegionsSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[GetAllRegions]: {0}", ex.Message);
            return Result<IEnumerable<RegionsSanitized>>.Failure(new Error("The GetAllRegionsQueryHandler failed", ex.Message));
        }
    }
}
