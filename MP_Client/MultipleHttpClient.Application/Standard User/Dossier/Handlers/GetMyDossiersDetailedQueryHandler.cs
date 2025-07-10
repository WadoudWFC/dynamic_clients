using MediatR;
using Microsoft.Extensions.Logging;
using MultipleHttpClient.Application.Dossier.Queries;
using MultipleHttpClient.Application.Standard_User.Dossier.Queries;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application;

public class GetMyDossiersDetailedQueryHandler : IRequestHandler<GetMyDossiersDetailedQuery, Result<MyDossiersDetailedResponse>>
{
    private readonly IMediator _mediator;
    private readonly ILogger<GetMyDossiersDetailedQueryHandler> _logger;

    public GetMyDossiersDetailedQueryHandler(IMediator mediator, ILogger<GetMyDossiersDetailedQueryHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result<MyDossiersDetailedResponse>> Handle(GetMyDossiersDetailedQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Use SearchDossierQuery directly with correct field validation
            var dossiersQuery = new SearchDossierQuery
            {
                UserId = request.UserId,
                RoleId = request.RoleId,
                ApplyFilter = false,
                Take = 50,
                Skip = 0,
                Field = "date_created",
                Order = "desc"
            };

            var dossiersResult = await _mediator.Send(dossiersQuery, cancellationToken);

            if (dossiersResult == null || !dossiersResult.IsSuccess)
            {
                return Result<MyDossiersDetailedResponse>.Failure(dossiersResult.Error);
            }

            // Get counts
            var countsQuery = new GetCountsQuery
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            };

            var countsResult = await _mediator.Send(countsQuery, cancellationToken);

            // FIXED: Access the dossiers collection properly
            var dossiers = dossiersResult.Value?.ToList() ?? new List<DossierSearchSanitized>();

            _logger.LogInformation("Processing {0} dossiers for detailed report for user {1}", dossiers.Count, request.UserId);

            // Group by status
            var groupedByStatus = dossiers
                .GroupBy(d => d.Status ?? "Unknown")
                .Select(g => new StatusGroup(
                    Status: g.Key,
                    Count: g.Count(),
                    Dossiers: g.Take(5).ToList()
                ))
                .OrderByDescending(g => g.Count)
                .ToList();

            // Group by demand type
            var groupedByType = dossiers
                .GroupBy(d => d.TypeDemande ?? "Unknown")
                .Select(g => new TypeGroup(
                    Type: g.Key,
                    Count: g.Count()
                ))
                .OrderByDescending(g => g.Count)
                .ToList();

            // Create response
            var response = new MyDossiersDetailedResponse(
                Summary: new MyDossiersDetailedSummary(
                    TotalDossiers: countsResult.IsSuccess ? countsResult.Value.TotalDossier : dossiers.Count,
                    TotalPending: countsResult.IsSuccess ? countsResult.Value.TotalDossierEncours : 0,
                    TotalProcessed: countsResult.IsSuccess ? countsResult.Value.TotalDossierTraiter : 0,
                    UserId: request.UserId,
                    ProfileType: GetProfileTypeName(request.RoleId),
                    AccessLevel: GetAccessLevelDescription(request.RoleId)
                ),
                Statistics: new MyDossiersStatistics(
                    ByStatus: groupedByStatus,
                    ByType: groupedByType,
                    RecentDossiers: dossiers.OrderByDescending(d => d.DateCreated).Take(10).ToList(),
                    OldestDossiers: dossiers.OrderBy(d => d.DateCreated).Take(5).ToList()
                )
            );

            _logger.LogInformation("Successfully generated detailed dossier report for user {0}", request.UserId);
            return Result<MyDossiersDetailedResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating detailed dossier report for user {0}", request.UserId);
            return Result<MyDossiersDetailedResponse>.Failure(new Error(Constants.DossierFail, "Failed to generate detailed report"));
        }
    }

    private string GetProfileTypeName(string profileId) => profileId switch
    {
        "1" => "Administrator",
        "2" => "Regional Administrator",
        "3" => "Standard User",
        _ => "Unknown"
    };

    private string GetAccessLevelDescription(string profileId) => profileId switch
    {
        "1" => "Full access to all dossiers",
        "2" => "Access to commercial division dossiers",
        "3" => "Access to own dossiers only",
        _ => "Limited access"
    };
}