using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, Result<IEnumerable<CitiesSanitized>>>
{
    private readonly IReferenceAglouDataService _referenceAglouDataService;
    private readonly ILogger<GetAllCitiesQueryHandler> _logger;
    public GetAllCitiesQueryHandler(IReferenceAglouDataService referenceAglouDataService, ILogger<GetAllCitiesQueryHandler> logger)
    {
        _referenceAglouDataService = referenceAglouDataService;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<CitiesSanitized>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _referenceAglouDataService.GetAllCitiesAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[GetAllCities]: {0} failed execution", nameof(GetAllCitiesQueryHandler));
                return Result<IEnumerable<CitiesSanitized>>.Failure(new Error("The GetAllCitiesQueryHandler failed", "Can't handle GetAllCitiesQueryHandler"));
            }
            _logger.LogInformation("[GetAllCities]: Successful operation!");
            return Result<IEnumerable<CitiesSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[GetAllCities]: {0}", ex.Message);
            return Result<IEnumerable<CitiesSanitized>>.Failure(new Error("The GetAllCitiesQueryHandler failed", ex.Message));
        }
    }
}
