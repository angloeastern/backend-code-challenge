using AEChallenge.Application.Constants;
using AEChallenge.Application.DTOs.Request;
using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AEChallenge.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/finance")]
    [Authorize(Roles = UserRoles.HighPrivilegeRoles)]
   
    public class FinanceController(IFinanceRepository financeRepository, ILogger<FinanceController> logger) : ControllerBase
    {
        private readonly IFinanceRepository _financeRepository = financeRepository;
        private readonly ILogger<FinanceController> _logger = logger;

        [HttpGet("summaryreport")]
        [EnableRateLimiting("ReportPolicy")]
        public async Task<IActionResult> GetFinancialSummaryReport([FromQuery] FinancialReportParameters parameters)
        {
            var (IsValid, ErrorMessage) = parameters.Validate();
            if (!IsValid)
            {
                _logger.LogWarning("GetFinancialSummaryReport validation failed: {Errors}", ErrorMessage);
                var errorResult = ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>.Invalid(
                   ErrorMessage
               );
                return BadRequest(errorResult);
            }

            var report = await _financeRepository.GetFinancialSummaryReportAsync(
                parameters.ShipCode, parameters.PeriodId);

            if (report.Data == null || !report.Data.Any())
            {
                var notfoundData = ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>.NotFound(
               );
                return NotFound(notfoundData);
            }

            return Ok(report);
        }
        [HttpGet("detailreport")]
        public async Task<IActionResult> GetFinancialDetailReport([FromQuery] FinancialReportParameters parameters)
        {
            var (IsValid, ErrorMessage) = parameters.Validate();

            if (!IsValid)
            {
                _logger.LogWarning("GetFinancialDetailReport validation failed: {Errors}", ErrorMessage);
                var errorResult = ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>.Invalid(
                   ErrorMessage
               );
                return BadRequest(errorResult);
            }

            var report = await _financeRepository.GetFinancialDetailReportAsync(
                parameters.ShipCode, parameters.PeriodId);

            if (report.Data == null || !report.Data.Any())
            {
                var notfoundData = ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>.NotFound(
               );
                return NotFound(notfoundData);
            }

            return Ok(report);
        }
    }

}