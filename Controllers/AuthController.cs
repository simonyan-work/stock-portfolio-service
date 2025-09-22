using Microsoft.AspNetCore.Mvc;
using StockPortfolioAPI.Models.Auth;
using StockPortfolioAPI.Services.Auth;

namespace StockPortfolioAPI.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase{
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(IAuthService authService, ILogger<AuthController> logger){
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Login to get JWT token
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request){
            try{
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var loginResponse = await _authService.LoginAsync(request);
                if (loginResponse == null)
                    return Unauthorized("Invalid username or password");
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        ///<summary>
        /// Test endpoint to verify JWT token (requires authentication)
        /// </summary>
        [HttpGet("test")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Test(){
            var username = User.Identity?.Name;
            var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            return Ok(new { Message = "Test endpoint accessed successfully", Username = username, Role = role, Claims = User.Claims.Select(c => new { c.Type, c.Value }) });
        }
    }
}