using MediatR;
using MutipleHttpClient.Domain;

namespace MultipleHttpClient.Application;

public class SearchHistoryQuery : IRequest<Result<IEnumerable<HistorySearchSanitized>>>
{
    public Guid DossierId { get; set; }
    public string Field { get; set; } = "date_created";
    public string Order { get; set; } = "desc";
    [NonNegative]
    public int Skip { get; set; } = 0;
    [NonNegative]
    public int Take { get; set; } = 10;
}
