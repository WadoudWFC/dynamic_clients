using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using MultipleHtppClient.Infrastructure;

namespace MultipleHtppClient.API;

[ApiController]
[Route("api/[controller]")]
public class PresentationController : ControllerBase
{
    private readonly IUseHttpService _useHttpService;

    public PresentationController(IUseHttpService useHttpService)
    {
        _useHttpService = useHttpService;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _useHttpService.GetAllProductsAsync();

        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }

        return Ok(response.Data);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var response = await _useHttpService.GetProductByIdAsync(id);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpGet("ticker")]
    public async Task<IActionResult> Ticker()
    {
        var response = await _useHttpService.TickerAsync();
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("Canlogin")]
    public async Task<ActionResult<ApiResponse<Aglou10001Response<AglouUser>>>> CanLogin([FromBody] CanTryLoginRequestBody email)
    {
        var response = await _useHttpService.CanTryLoginAsync(email);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestBody loginRequest)
    {
        var response = await _useHttpService.LoginAsync(loginRequest);
        if (!response.IsSuccess)
        {
            return StatusCode((int)response.StatusCode, response.ErrorMessage);
        }
        return Ok(response.Data);
    }
}
