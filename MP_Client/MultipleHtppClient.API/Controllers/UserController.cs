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
    [HttpPost("CanTryLogin")]
    public async Task<ActionResult<Result<SanitizedUserResponse>>> CanTryLogin([FromBody] CanTryLoginCommand command) => Ok(await _mediator.Send(command));
    [EnableRateLimiting("login")]
    [HttpPost("UpdatePassword")]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> UpdatePassword([FromBody] UpdatePasswordCommand command) => Ok(await _mediator.Send(command));
    [HttpPost("Logout")]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> Logout([FromBody] LogoutCommand command) => Ok(await _mediator.Send(command));
    [HttpPost("ForgetPassword")]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> ForgetPassword([FromBody] ForgetPasswordCommand command) => Ok(await _mediator.Send(command));
    [HttpPost("Register")]
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
    [RequireAdminOrRegional]
    public async Task<ActionResult<Result<LoadUserResponseSanitized>>> GetUserById([FromBody] LoadUserCommand command) => Ok(await _mediator.Send(command));
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("user_id")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}
