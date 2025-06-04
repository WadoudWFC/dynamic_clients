using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class GetTypeBienQueryHandler : IRequestHandler<GetTypeBienQuery, Result<IEnumerable<TypeBienSanitized>>>
{
    private readonly IReferenceAglouDataService _referenceAglouDataService;
    private readonly ILogger<GetTypeBienQueryHandler> _logger;
    public GetTypeBienQueryHandler(IReferenceAglouDataService referenceAglouDataService, ILogger<GetTypeBienQueryHandler> logger)
    {
        _referenceAglouDataService = referenceAglouDataService;
        _logger = logger;
    }
    public async Task<Result<IEnumerable<TypeBienSanitized>>> Handle(GetTypeBienQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _referenceAglouDataService.GetTypeBienAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[GetTypeBien]: {0} failed execution", nameof(GetTypeBienQueryHandler));
                return Result<IEnumerable<TypeBienSanitized>>.Failure(new Error("The GetTypeBienQueryHandler failed", "Can't handle GetTypeBienQueryHandler"));
            }
            _logger.LogInformation("[GetTypeBien]: Successful operation!");
            return Result<IEnumerable<TypeBienSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[GetTypeBien]: {0}", ex.Message);
            return Result<IEnumerable<TypeBienSanitized>>.Failure(new Error("The GetTypeBienQueryHandler failed", ex.Message));
        }
    }
}
