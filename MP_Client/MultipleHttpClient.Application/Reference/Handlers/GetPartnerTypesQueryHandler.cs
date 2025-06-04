using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetPartnerTypesQueryHandler : IRequestHandler<GetPartnerTypesQuery, Result<IEnumerable<PartnerTypeSanitized>>>
{
    private readonly IReferenceAglouDataService _referenceAglouDataService;
    private readonly ILogger<GetPartnerTypesQueryHandler> _logger;
    public GetPartnerTypesQueryHandler(IReferenceAglouDataService referenceAglouDataService, ILogger<GetPartnerTypesQueryHandler> logger)
    {
        _referenceAglouDataService = referenceAglouDataService;
        _logger = logger;
    }
    public async Task<Result<IEnumerable<PartnerTypeSanitized>>> Handle(GetPartnerTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _referenceAglouDataService.GetPartnerTypesAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[GetPartnerTypes]: {0} failed execution", nameof(GetPartnerTypesQueryHandler));
                return Result<IEnumerable<PartnerTypeSanitized>>.Failure(new Error("The GetPartnerTypesQueryHandler failed", "Can't handle GetPartnerTypesQueryHandler"));
            }
            _logger.LogInformation("[GetPartnerTypes]: Successful operation!");
            return Result<IEnumerable<PartnerTypeSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[GetPartnerTypes]: {0}", ex.Message);
            return Result<IEnumerable<PartnerTypeSanitized>>.Failure(new Error("The GetPartnerTypesQueryHandler failed", ex.Message));
        }
    }
}
