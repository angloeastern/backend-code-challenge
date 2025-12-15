using AEChallenge.Application.DTOs;
using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Interfaces;
using AEChallenge.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AEChallenge.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/crew")]
    public class CrewController(ICrewRepository crewRepository, ILogger<CrewController> logger) : ControllerBase
    {
        private readonly ICrewRepository _crewRepository = crewRepository;
        private readonly ILogger<CrewController> _logger = logger;

        [HttpGet("crewlist")]
        public async Task<IActionResult> GetCrewList([FromQuery] CrewListParameters parameters)
        {
            var (IsValid, ErrorMessage) = parameters.Validate();
            if (!IsValid)
            {
                _logger.LogWarning("GetCrewList validation failed: {Errors}", ErrorMessage);
                var errorResult = ApiResponseResult<CrewListReadModel>.Invalid(
                   ErrorMessage
               );
                return BadRequest(errorResult);
            }

            var crewList = await _crewRepository.GetCrewListAsync(
               parameters);
            if (!crewList.IsSuccess)
            {
                _logger.LogWarning("GetCrewList failed: {Message}", crewList.Message);
                _ = ApiResponseResult<CrewListReadModel>.Invalid(
                    crewList.Message
                );
                return BadRequest(crewList);
            }
            if (crewList.Data == null || !crewList.Data.Any())
            {
                var errorResult = ApiResponseResult<CrewListReadModel>.NotFound();

                return NotFound(errorResult);
            }
            return Ok(crewList);
        }
    }
}