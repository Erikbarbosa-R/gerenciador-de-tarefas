using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Commands.AuthenticateUser;
using TaskManager.Application.Users.Commands.CreateUser;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand request)
    {
        var user = await _mediator.Send(request);
        
        if (user.IsFailure)
            return BadRequest(user.Error);

        return Ok(user.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserCommand authRequest)
    {
        var token = await _mediator.Send(authRequest);
        
        if (token.IsFailure)
            return Unauthorized(token.Error);

        return Ok(token.Value);
    }
}
