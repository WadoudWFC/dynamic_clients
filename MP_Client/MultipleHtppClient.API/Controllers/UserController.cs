using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultipleHttpClient.Application;
using MultipleHttpClient.Application.Users.Commands.Can_Try_Login;
using MultipleHttpClient.Application.Users.Commands.ForgetPassword;
using MultipleHttpClient.Application.Users.Commands.Logout;
using MutipleHttpClient.Domain;

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
    [HttpPost("Login")]
    public async Task<ActionResult<Result<SanitizedLoginResponse>>> Login([FromBody] LoginCommand command) => Ok(await _mediator.Send(command));

    [HttpPost("CanTryLogin")]
    public async Task<ActionResult<Result<SanitizedUserResponse>>> CanTryLogin([FromBody] CanTryLoginCommand command) => Ok(await _mediator.Send(command));

    [HttpPost("UpdatePassword")]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> UpdatePassword([FromBody] UpdatePasswordCommand command) => Ok(await _mediator.Send(command));

    [HttpPost("Logout")]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> Logout([FromBody] LogoutCommand command) => Ok(await _mediator.Send(command));

    [HttpPost("ForgetPassword")]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> ForgetPassword([FromBody] ForgetPasswordCommand command) => Ok(await _mediator.Send(command));
    
    [HttpPost("Register")]
    public async Task<ActionResult<Result<SanitizedBasicResponse>>> RegisterUser([FromBody] RegisterUserCommand command) => Ok(await _mediator.Send(command));

}
