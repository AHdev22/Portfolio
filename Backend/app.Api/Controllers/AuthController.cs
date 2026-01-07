using app.Application.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Api.Controllers
{
    [Route("api/v1/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var authResult = await _mediator.Send(command);

                return Ok(new { Token = authResult.Token });
            }
            catch (UnauthorizedAccessException)
            {
                return BadRequest(new { title = "Invalid username or password" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { title = "An internal server error occurred." });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout initiated. Client should clear token." });
        }

        [HttpGet("check")]
        [Authorize]
        public IActionResult CheckSession()
        {
            return Ok(new { message = "Authenticated" });
        }
    }
}