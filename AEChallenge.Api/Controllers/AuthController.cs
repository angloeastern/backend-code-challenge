using Microsoft.AspNetCore.Mvc;
using AEChallenge.Application.Interfaces;
using AEChallenge.Application.DTOs;
using System.Threading.Tasks;
using AEChallenge.Application.DTOs.Response;

namespace AEChallenge.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController(IAuthRepository authRepository, IJwtService jwtService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IJwtService _jwtService = jwtService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto credentials)
        {
            var (IsValid, ErrorMessage) = credentials.Validate();
            if (!IsValid)
            {
                _logger.LogWarning("Login validation failed for user {Username}: {Errors}", credentials.Username, ErrorMessage);
                var errorResult = ApiResponseResult<UserLoginDto>.Invalid(
                    ErrorMessage
                );
                return BadRequest(errorResult);
            }
            var authenticate = await _authRepository.AuthenticateAsync(credentials.Username, credentials.Password);

            if (!authenticate.IsSuccess)
            {
                _logger.LogWarning("Authentication failed for user {Username}: {Message}", credentials.Username, authenticate.Message);
                var errorResult = ApiResponseResult<UserLoginDto>.Invalid(
                    authenticate.Message
                );
                return BadRequest(errorResult);
            }

            var token = _jwtService.GenerateToken(authenticate.Data);
            var tokenDto = new TokenResponseDto { Token = token, User = authenticate.Data };
            _logger.LogInformation("User {Username} logged in successfully.", credentials.Username);
            return Ok(ApiResponseResult<TokenResponseDto>.Success(tokenDto));
        }
    }
}