using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Interfaces;
using AEChallenge.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AEChallenge.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ship")]
    [ApiController]
    public class ShipController(IShipRepository shipRepository, ILogger<ShipController> logger) : ControllerBase
    {
        private readonly IShipRepository _shipRepository = shipRepository;
        private readonly ILogger<ShipController> _logger = logger;

        [HttpPost()]
        public async Task<IActionResult> CreateShip([FromBody] ShipCreateDto shipDto)
        {
            var (IsValid, ErrorMessage) = shipDto.Validate();
            if (!IsValid)
            {
                _logger.LogWarning("Ship creation failed validation: {Errors}", ErrorMessage);
                var errorResult = ApiResponseResult<ShipReadModel>.Invalid(
                    ErrorMessage
                );
                return BadRequest(errorResult);
            }
            var userResult = await _shipRepository.AddShipsync(shipDto);

            if (!userResult.IsSuccess)
            {
                _logger.LogWarning("Ship creation failed: {Message}", userResult.Message);
                _ = ApiResponseResult<ShipReadModel>.Invalid(
                    userResult.Message
                );
                return BadRequest(userResult);
            }
            _logger.LogInformation("Ship created successfully: {ShipCode}", shipDto.ShipCode);
            return Ok(userResult);
        }

        [HttpGet("shiplist")]
        public async Task<IActionResult> GetShipList([FromQuery] ShipListParameters parameters)
        {
            var (IsValid, ErrorMessage) = parameters.Validate();
            if (!IsValid)
            {
                _logger.LogWarning("GetShipList failed validation: {Errors}", ErrorMessage);
                var errorResult = ApiResponseResult<IEnumerable<ShipReadModel>>.Invalid(
                   ErrorMessage
               );
                return BadRequest(errorResult);
            }

            var list = await _shipRepository.GetShipAsync(parameters);

            if (list.Data == null || !list.Data.Any())
            {
                var notfoundData = ApiResponseResult<IEnumerable<ShipReadModel>>.NotFound(
               );
                return NotFound(notfoundData);
            }

            return Ok(list);
        }

        [HttpPost("assignment")]
        public async Task<IActionResult> CreateShipAssignment([FromBody] ShipAssignmentDto shipDto)
        {
            var (IsValid, ErrorMessage) = shipDto.Validate();
            if (!IsValid)
            {
                _logger.LogWarning("Ship assignment validation failed: {Errors}", ErrorMessage);
                var errorResult = ApiResponseResult<ShipAssignmentReadModel>.Invalid(
                    ErrorMessage
                );
                return BadRequest(errorResult);
            }
            var userResult = await _shipRepository.AssignShipToUserAsync(shipDto);

            if (!userResult.IsSuccess)
            {
                _ = ApiResponseResult<ShipAssignmentReadModel>.Invalid(
                    userResult.Message
                );
                return BadRequest(userResult);
            }
            return Ok(userResult);
        }
        [HttpGet("shipbyuser/{userId}")]
        public async Task<IActionResult> CreateShipAssignment([FromRoute] int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("GetAssignmentShipByUser failed: Invalid UserId {UserId}", userId);
                var errorResult = ApiResponseResult<ShipAssignmentReadModel>.Invalid(
                    "Invalid UserId provided."
                );
                return BadRequest(errorResult);
            }
            var userResult = await _shipRepository.GetAssignmentShipByUser(userId);

            if (!userResult.IsSuccess)
            {
                _ = ApiResponseResult<ShipAssignmentReadModel>.Invalid(
                    userResult.Message
                );
                return BadRequest(userResult);
            }
            return Ok(userResult);
        }
    }
}
