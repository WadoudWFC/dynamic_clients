using System.Formats.Asn1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MultipleHttpClient.Application;
using MultipleHttpClient.Application.Services.Security;
using MultipleHttpClient.Application.Users.Commands.Can_Try_Login;
using MultipleHttpClient.Application.Users.Commands.ForgetPassword;
using MultipleHttpClient.Application.Users.Commands.LoadUser;
using MultipleHttpClient.Application.Users.Commands.Logout;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Users;

namespace MultipleHtppClient.API;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [EnableRateLimiting("login")]
    [HttpPost("Login")]
    public async Task<ActionResult<Result<SanitizedLoginResponse>>> Login([FromBody] LoginCommand command) => Ok(await _mediator.Send(command));
    [EnableRateLimiting("login")]
    [HttpPost("CanTryLogin")]
    public async Task<ActionResult<Result<SanitizedUserResponse>>> CanTryLogin([FromBody] CanTryLoginCommand command) => Ok(await _mediator.Send(command));
    [HttpPost("UpdatePassword")]
    [Authorize]
    [EnableRateLimiting("passwordUpdate")]
    [RequireProfile(1, 2, 3)]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> UpdatePassword([FromBody] UpdatePasswordCommand command)
    {
        var authenticatedUserId = GetCurrentUserId();

        if (authenticatedUserId == Guid.Empty)
        {
            return Unauthorized(new { error = "Invalid user context" });
        }
        var secureCommand = new UpdatePasswordCommand(authenticatedUserId, command.NewPassword);
        var result = await _mediator.Send(secureCommand);
        return Ok(result);
    }
    [HttpPost("Logout")]
    [Authorize]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> Logout([FromBody] LogoutCommand command)
    {
        var authenticatedUserId = GetCurrentUserId();

        if (command.UserId != authenticatedUserId)
        {
            return Forbid("Cannot logout other users");
        }
        var secureCommand = new LogoutCommand(authenticatedUserId);
        var result = await _mediator.Send(secureCommand);
        return Ok(result);
    }
    [HttpPost("ForgetPassword")]
    [EnableRateLimiting("passwordReset")]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> ForgetPassword([FromBody] ForgetPasswordCommand command) => Ok(await _mediator.Send(command));
    [HttpPost("Register")]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> RegisterUser([FromBody] RegisterUserCommand command) => Ok(await _mediator.Send(command));
    [HttpGet("GetCurrentUser")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var query = new LoadUserCommand
        {
            UserId = GetCurrentUserId()
        };
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    [HttpPost("User")]
    [Authorize]
    [RequireAdminOrRegional]
    public async Task<ActionResult<Result<LoadUserResponseSanitized>>> GetUserById([FromBody] LoadUserCommand command)
    {
        var currentUserProfile = GetCurrentInternalProfileId();
        var currentUserId = GetCurrentUserId();
        if (command.UserId == currentUserId)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        if (currentUserProfile != "1" && currentUserProfile != "2")
        {
            return Forbid("Insufficient permissions to access other users' data");
        }
        var adminResult = await _mediator.Send(command);
        return Ok(adminResult);
    }
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("user_id")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
    [HttpGet("GetCurrentUserProfileId")]
    [Authorize]
    public async Task<ActionResult<UserProfileIdResponse>> GetCurrentUserProfileIdLightweight()
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var profileIdClaim = GetCurrentInternalProfileId();

            if (currentUserId == Guid.Empty)
            {
                return BadRequest(new { error = "Invalid user context" });
            }

            if (string.IsNullOrEmpty(profileIdClaim) || !int.TryParse(profileIdClaim, out var profileId))
            {
                return BadRequest(new { error = "Profile information not available in token" });
            }

            var response = new UserProfileIdResponse
            {
                UserId = currentUserId,
                ProfileId = profileId,
                ProfileName = GetProfileName(profileId),
                AccessLevel = GetAccessLevel(profileId),
                RetrievedAt = DateTime.UtcNow
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "An error occurred while retrieving profile information" });
        }
    }
    private string GetCurrentInternalProfileId()
    {
        return User.FindFirst("internal_profile_id")?.Value ?? "3";
    }
    private static string GetProfileName(int profileId) => profileId switch
    {
        1 => "Administrator",
        2 => "Regional Administrator",
        3 => "Standard User",
        _ => "Unknown Profile"
    };

    private static string GetAccessLevel(int profileId) => profileId switch
    {
        1 => "Full Access - All system functions",
        2 => "Regional Access - Limited to assigned regions",
        3 => "Standard Access - Own data only",
        _ => "No Access"
    };
}
