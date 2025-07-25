﻿using MultipleHtppClient.Infrastructure.HTTP.REST;

namespace MultipleHtppClient.Infrastructure.HTTP.Interfaces;

public interface IHttpClientService
{
    Task<ApiResponse<TResponse>> SendAsync<TRequest, TResponse>(ApiRequest<TRequest> apiRequest, CancellationToken cancellationToken = default) where TRequest : class;
    Task<ApiResponse<Aglou10001Response<TResponse>>> SendNestedJsonAsync<TRequest, TResponse>(ApiRequest<TRequest> apiRequest) where TRequest : class;
}
