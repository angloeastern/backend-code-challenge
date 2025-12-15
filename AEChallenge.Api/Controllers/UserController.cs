using AEChallenge.Application.Constants;
using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Interfaces;
using AEChallenge.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AEChallenge.Api.Controllers
{
    [Authorize(Roles = UserRoles.Administrator)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    public class UserController(IUserRepository UserRepository, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IUserRepository _userRepository = UserRepository;
        private readonly ILogger<UserController> _logger = logger;

        [HttpPost()]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userDto)
        {
            var (IsValid, ErrorMessage) = userDto.Validate();
            if (!IsValid)
            {
                _logger.LogWarning("User creation validation failed: {Errors}", ErrorMessage);
                var errorResult = ApiResponseResult<UserCreationReadModel>.Invalid(
                    ErrorMessage
                );
                return BadRequest(errorResult);
            }
            var userResult = await _userRepository.AddUserAsync(userDto);

            if (!userResult.IsSuccess)
            {
                _logger.LogWarning("User creation failed: {Message}", userResult.Message);
                _ = ApiResponseResult<UserCreateDto>.Invalid(
                    userResult.Message
                );
                return BadRequest(userResult);
            }
            _logger.LogInformation("User created successfully: {UserName}", userDto.UserName);
            return Ok(userResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersList([FromQuery] BaseQueryParameters parameters)
        {
            var (IsValid, ErrorMessage) = parameters.Validate();
            if (!IsValid)
            {
                _logger.LogWarning("GetUsersList validation failed: {Errors}", ErrorMessage);
                var errorResult = ApiResponseResult<UserCreationReadModel>.Invalid(
                   ErrorMessage
               );
                return BadRequest(errorResult);
            }

            var list = await _userRepository.GetUserAsync(
               parameters);
            if (!list.IsSuccess)
            {
                _ = ApiResponseResult<UserCreationReadModel>.Invalid(
                    list.Message
                );
                return BadRequest(list);
            }
            if (list.Data == null || !list.Data.Any())
            {
                var errorResult = ApiResponseResult<UserCreationReadModel>.NotFound();

                return NotFound(errorResult);
            }
            return Ok(list);
        }
    }
}