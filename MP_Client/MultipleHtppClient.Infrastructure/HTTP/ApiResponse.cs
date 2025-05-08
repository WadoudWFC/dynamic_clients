using System.Net;

namespace MultipleHtppClient.Infrastructure;

public sealed class ApiResponse<TResponse>
{
    public bool IsSuccess { get; private init; }
    public TResponse? Data { get; private init; }
    public string? ErrorMessage { get; private init; }
    public HttpStatusCode StatusCode { get; private init; }
    public bool IsCancelled { get; private init; }
    public static ApiResponse<TResponse> Success(TResponse? data, HttpStatusCode httpStatusCode = HttpStatusCode.OK) => new ApiResponse<TResponse> { IsSuccess = true, Data = data, StatusCode = httpStatusCode };
    public static ApiResponse<TResponse> Error(string errorMessage, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) => new ApiResponse<TResponse> { IsSuccess = false, ErrorMessage = errorMessage, StatusCode = httpStatusCode };
    public static ApiResponse<TResponse> Cancelled() => new ApiResponse<TResponse> { IsSuccess = false, ErrorMessage = "Request was cancelled", StatusCode = HttpStatusCode.RequestTimeout, IsCancelled = true };
}
