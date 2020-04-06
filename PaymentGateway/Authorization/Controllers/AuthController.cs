using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Authorization;
using PaymentGateway.Authorization.Services;

namespace PaymentGateway.Authorization.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private ILogger<AuthController> _logger;

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]Login credentials)
        {
            var userPayload = await _authService.LoginAsync(credentials.UserName, credentials.Password);

            if (userPayload == null)
            {
                _logger.LogWarning($"Login failed for user {credentials.UserName}.");

                return BadRequest("Invalid credentials.");
            }

            _logger.LogInformation($"User {credentials.UserName} successfully logged in.");

            return Ok(userPayload);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]RegisterModel registerModel)
        {
            try
            {
                var newUser = await _authService.SaveAsync(registerModel.UserName, registerModel.Password);

                _logger.LogInformation($"User {registerModel.UserName} succesfully created.");
                return CreatedAtAction(nameof(Register), newUser);
            } catch(Exception e)
            {
                _logger.LogError(e, $"Error while trying to create user for {registerModel.UserName}.", registerModel);
                return BadRequest(registerModel);
            }
        }


        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
    }
}