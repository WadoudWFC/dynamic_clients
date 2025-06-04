using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetActivitiesQueryHandler : IRequestHandler<GetAllActivitiesQuery, Result<IEnumerable<ActivityNatureSanitized>>>
{
    private readonly IReferenceAglouDataService _referenceAglouDataService;
    private readonly ILogger<GetActivitiesQueryHandler> _logger;
    public GetActivitiesQueryHandler(IReferenceAglouDataService referenceAglouDataService, ILogger<GetActivitiesQueryHandler> logger)
    {
        _referenceAglouDataService = referenceAglouDataService;
        _logger = logger;
    }
    public async Task<Result<IEnumerable<ActivityNatureSanitized>>> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _referenceAglouDataService.GetAllActivitiesAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[GetAllActivities]: {0} failed execution", nameof(GetActivitiesQueryHandler));
                return Result<IEnumerable<ActivityNatureSanitized>>.Failure(new Error("The GetActivitiesQueryHandler failed", "Can't handle GetAllActivities"));
            }
            _logger.LogInformation("[GetAllActivities]: Successful operation!");
            return Result<IEnumerable<ActivityNatureSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[GetAllActivities]: {0}", ex.Message);
            return Result<IEnumerable<ActivityNatureSanitized>>.Failure(new Error("The GetActivitiesQueryHandler failed", ex.Message));
        }
    }
}
