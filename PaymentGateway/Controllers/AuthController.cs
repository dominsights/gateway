using System.Threading.Tasks;
using BackendTraining.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Authorization;
using PaymentGateway.Authorization.Services;

namespace PaymentGateway.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService _authService;

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]Login credentials)
        {
            var userPayload = await _authService.LoginAsync(credentials.UserName, credentials.Password);

            if(userPayload == null)
            {
                return BadRequest("Invalid credentials.");
            }

            return Ok(userPayload);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]RegisterModel registerModel)
        {
            var newUser = await _authService.SaveAsync(registerModel.UserName, registerModel.Password);
            return CreatedAtAction(nameof(Register), newUser);
        }


        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
    }
}