using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Models.User_Account.Responses;
using MultipleHtppClient.Infrastructure.HTTP.REST;

namespace MultipleHttpClient.Application;

public interface IHttpUserAglou
{
    Task<ApiResponse<Aglou10001Response<AglouUser>>> CanTryLoginAsync(CanTryLoginRequestBody email);
    Task<ApiResponse<Aglou10001Response<AglouLoginResponse>>> LoginAsync(LoginRequestBody loginRequestBody);
    Task<ApiResponse<Aglou10001Response<object>>> LogoutAsync(LogoutRequestBody loginRequestBody);
    Task<ApiResponse<Aglou10001Response<object>>> ForgetPasswordAsync(ForgetPasswordRequestBody forgetPasswordRequestBody);
    Task<ApiResponse<Aglou10001Response<object>>> UpdatePasswordAsync(UpdatePasswordRequestBody updatePasswordRequestBody);
}
