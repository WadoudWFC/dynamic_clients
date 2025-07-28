using MultipleHtppClient.Infrastructure;
using MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests;
using MultipleHttpClient.Application.Interfaces.User;
using MultipleHttpClient.Application.Users.Commands.Can_Try_Login;
using MultipleHttpClient.Application.Users.Commands.ForgetPassword;
using MultipleHttpClient.Application.Users.Commands.LoadUser;
using MultipleHttpClient.Application.Users.Commands.Logout;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Users;

namespace MultipleHttpClient.Application.Services.User;

public class UserAglouService : IUserAglouService
{
    private readonly IHttpUserAglou _httpUserAglou;
    private readonly IIdMappingService _idMappingService;
    private readonly IReferenceDataMappingService _referenceDataMappingService;
    public UserAglouService(IHttpUserAglou httpUserAglou, IIdMappingService idMappingService, IReferenceDataMappingService referenceDataMappingService)
    {
        _httpUserAglou = httpUserAglou;
        _idMappingService = idMappingService;
        _referenceDataMappingService = referenceDataMappingService;
    }
    public async Task<Result<SanitizedUserResponse>> CanTryLoginAsync(CanTryLoginCommand command)
    {
        CanTryLoginRequestBody request = new CanTryLoginRequestBody(command.Email);
        var response = await _httpUserAglou.CanTryLoginAsync(request);
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<SanitizedUserResponse>.Failure(new Error(Constants.UserFail, "Cannot try login"));
        }
        var externalResponse = response.Data.Data;
        var userId = externalResponse.UserId;
        if (!int.TryParse(userId, out int Id))
        {
            return Result<SanitizedUserResponse>.Failure(new Error(Constants.UserFail, "Invalid User Id!"));
        }
        var userGuid = _idMappingService.GetGuidForUserId(Id);
        return Result<SanitizedUserResponse>.Success(new SanitizedUserResponse(UserId: userGuid, FirstName: externalResponse.FirstName, LastName: externalResponse.LastName,
                                                     IsActive: externalResponse.IsUserActive, externalResponse.TryingCounter,
                                                     IsPasswordUpdateRequired: externalResponse.IsPasswordUpdated));
    }

    public async Task<Result<SanitizedLoginResponse>> LoginAsync(LoginCommand command)
    {
        LoginRequestBody request = new LoginRequestBody(command.Email, command.Password);
        var response = await _httpUserAglou.LoginAsync(request);
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<SanitizedLoginResponse>.Failure(new Error(Constants.UserFail, "Login failed!"));
        }
        var externalResponse = response.Data.Data;
        var userId = externalResponse.UserId;
        var usrGuid = _idMappingService.GetGuidForUserId(userId);
        return Result<SanitizedLoginResponse>.Success(new SanitizedLoginResponse(UserId: usrGuid, FirstName: externalResponse.FirstName,
                                                    LastName: externalResponse.LastName, IsPasswordUpdated: externalResponse.IsPasswordUpdated,
                                                    BearerToken: externalResponse.BearerKey));
    }

    public async Task<Result<SanitizedBasicResponse>> LogoutAsync(LogoutCommand command)
    {
        var userId = _idMappingService.GetUserIdForGuid(command.UserId);
        if (userId == null || userId is not int)
        {
            return Result<SanitizedBasicResponse>.Failure(new Error(Constants.UserFail, Constants.InvalidUser));
        }
        LogoutRequestBody request = new LogoutRequestBody { Id = (int)userId };
        var response = await _httpUserAglou.LogoutAsync(request);
        if (!response.IsSuccess)
        {
            return Result<SanitizedBasicResponse>.Failure(new Error(Constants.UserFail, "Logout failed!"));
        }
        var externalResponse = response.Data;
        return Result<SanitizedBasicResponse>.Success(new SanitizedBasicResponse(true, externalResponse.Message));

    }
    public async Task<Result<SanitizedBasicResponse>> ForgetPasswordAsync(ForgetPasswordCommand command)
    {
        ForgetPasswordRequestBody request = new ForgetPasswordRequestBody { Mail = command.Email };
        var response = await _httpUserAglou.ForgetPasswordAsync(request);
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<SanitizedBasicResponse>.Failure(new Error(Constants.UserFail, "Forget Password failed!"));
        }
        var externalResponse = response.Data;
        return Result<SanitizedBasicResponse>.Success(new SanitizedBasicResponse(true, externalResponse.Message));
    }

    public async Task<Result<SanitizedBasicResponse>> UpdatePasswordAsync(UpdatePasswordCommand command)
    {
        var userId = _idMappingService.GetUserIdForGuid(command.UserId);
        if (userId == null || userId is not int)
        {
            return Result<SanitizedBasicResponse>.Failure(new Error(Constants.UserFail, Constants.InvalidUser));
        }
        UpdatePasswordRequestBody request = new UpdatePasswordRequestBody { Id = (int)userId, Password = command.NewPassword };
        var response = await _httpUserAglou.UpdatePasswordAsync(request);
        if (!response.IsSuccess)
        {
            return Result<SanitizedBasicResponse>.Failure(new Error(Constants.UserFail, "Update password failed!"));
        }
        var externalResponse = response.Data;
        return Result<SanitizedBasicResponse>.Success(new SanitizedBasicResponse(true, externalResponse.Message));
    }

    public async Task<Result<SanitizedBasicResponse>> RegisterUserAsync(RegisterUserCommand command)
    {
        RegisterUserRequestBody request = new RegisterUserRequestBody
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Address = command.Address,
            MailAddress = command.MailAddress,
            PhoneNumber = command.PhoneNumber,
            ProfileId = 3,
            Gender = command.Gender
        };
        var response = await _httpUserAglou.RegisterUserAsync(request);
        if (!response.IsSuccess || response.Data == null)
        {
            return Result<SanitizedBasicResponse>.Failure(new Error(Constants.UserFail, "Registration failed!"));
        }
        var externalResponse = response.Data;
        return Result<SanitizedBasicResponse>.Success(new SanitizedBasicResponse(true, externalResponse.Message));
    }

    public async Task<Result<LoadUserResponseSanitized>> GetUserByIdAsync(LoadUserCommand command)
    {
        var userId = _idMappingService.GetUserIdForGuid(command.UserId);
        if (userId == null || userId is not int)
        {
            return Result<LoadUserResponseSanitized>.Failure(new Error(Constants.UserFail, Constants.UserMappingError));
        }
        var response = await _httpUserAglou.LoadUserAsync(new LogoutRequestBody { Id = userId.Value });
        if (!response.IsSuccess || response.Data?.Data == null)
        {
            return Result<LoadUserResponseSanitized>.Failure(new Error(Constants.UserFail, Constants.NoDataError));
        }
        var userData = response.Data.Data;
        return Result<LoadUserResponseSanitized>.Success(new LoadUserResponseSanitized
        {
            User = command.UserId,
            FirstName = userData.FirstName,
            LastName = userData.LastName,
            Gender = userData.Gender,
            EmailAddress = userData.Email,
            PhoneNumber = userData.Phone,
            Profile_Id = _referenceDataMappingService.GetOrCreateGuidForReferenceId(userData.ProfileId, Constants.Profile),
            EntityId = userData.EntityId,
            DecoupageCommercialId = userData.CommercialSegmentationId,
            ParentUserId = userData.ParentUserId,
            IsActive = userData.IsActive,
            ImageUrl = userData.ImageUrl,
            Image = userData.Image,
            InternalId = userData.Id,
            InternalProfileId = userData.ProfileId
        });
    }
}