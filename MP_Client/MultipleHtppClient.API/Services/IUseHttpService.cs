using MultipleHtppClient.Infrastructure;

namespace MultipleHtppClient.API;

public interface IUseHttpService
{
    Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync();
    Task<ApiResponse<Product>> GetProductByIdAsync(int id);
    Task<ApiResponse<object>> TickerAsync();
    Task<ApiResponse<Aglou10001Response<AglouUser>>> CanTryLoginAsync(CanTryLoginRequestBody email);
    Task<ApiResponse<Aglou10001Response<AglouLoginResponse>>> LoginAsync(LoginRequestBody loginRequestBody);
}