using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetAllPacksQueryHandler : IRequestHandler<GetAllPackQuery, Result<IEnumerable<PackSanitized>>>
{
    private readonly IReferenceAglouDataService _referenceAglouDataService;
    private readonly ILogger<GetAllPacksQueryHandler> _logger;
    public GetAllPacksQueryHandler(IReferenceAglouDataService referenceAglouDataService, ILogger<GetAllPacksQueryHandler> logger)
    {
        _referenceAglouDataService = referenceAglouDataService;
        _logger = logger;
    }
    public async Task<Result<IEnumerable<PackSanitized>>> Handle(GetAllPackQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _referenceAglouDataService.GetAllPacksAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[GetAllPacks]: {0} failed execution", nameof(GetAllPacksQueryHandler));
                return Result<IEnumerable<PackSanitized>>.Failure(new Error("The GetAllPacksQueryHandler failed", "Can't handle GetAllPacksQueryHandler"));
            }
            _logger.LogInformation("[GetAllPacks]: Successful operation!");
            return Result<IEnumerable<PackSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[GetAllPacks]: {0}", ex.Message);
            return Result<IEnumerable<PackSanitized>>.Failure(new Error("The GetAllPacksQueryHandler failed", ex.Message));

        }
    }
}
