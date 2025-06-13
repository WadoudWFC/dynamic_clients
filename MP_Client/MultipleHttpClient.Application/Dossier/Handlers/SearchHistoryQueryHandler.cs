using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class SearchHistoryQueryHandler : IRequestHandler<SearchHistoryQuery, Result<IEnumerable<HistorySearchSanitized>>>
{
    private readonly IDossierAglouService _dossierAglouService;
    private readonly ILogger<SearchHistoryQueryHandler> _logger;

    public SearchHistoryQueryHandler(IDossierAglouService dossierAglouService, ILogger<SearchHistoryQueryHandler> logger)
    {
        _dossierAglouService = dossierAglouService;
        _logger = logger;
    }
    public async Task<Result<IEnumerable<HistorySearchSanitized>>> Handle(SearchHistoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dossierAglouService.SearchHistroyAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                _logger.LogError("[SearchHistroy]: {0} failed execution", nameof(SearchHistoryQueryHandler));
                return Result<IEnumerable<HistorySearchSanitized>>.Failure(new Error("The SearchHistoryQueryHandler failed", "Unable to Search History!"));
            }
            _logger.LogInformation("[SearchHistroy]: Successful operation!");
            return Result<IEnumerable<HistorySearchSanitized>>.Success(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("[SearchHistroy]: {0}", ex.Message);
            return Result<IEnumerable<HistorySearchSanitized>>.Failure(new Error("The SearchHistoryQueryHandler failed", ex.Message));
        }
    }
}
