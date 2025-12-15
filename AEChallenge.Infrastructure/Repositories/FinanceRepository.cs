using AEChallenge.Application.DTOs.Response;
using AEChallenge.Application.Extensions;
using AEChallenge.Application.Interfaces;
using AEChallenge.Infrastructure.Data;
using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AEChallenge.Infrastructure.Repositories
{
 
    /// Mengimplementasikan kontrak IFinanceRepository menggunakan EF Core raw SQL (Stored Procedure).
    /// </summary>
    public class FinanceRepository(AppDbContext context, ILogger<FinanceRepository> logger) : IFinanceRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<FinanceRepository> _logger = logger;

        public async Task<ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>> GetFinancialSummaryReportAsync(
            string shipCode, string periodId)
        {
            var parameters = new[]
            {
                new SqlParameter("@ShipCode", shipCode),
                new SqlParameter("@PeriodId", periodId)
            };

            string sql = "EXEC SP_FinancialExpenseSummaryReport @ShipCode, @PeriodId";
            try
            {
                var reportList = await _context.Set<FinancialReportSummaryReadModel>()
                                 .FromSqlRaw(sql, parameters)
                                 .AsNoTracking()
                                 .ToListAsync();
                if (reportList == null || reportList.Count == 0)
                {
                    _logger.LogWarning("No data found for Financial Summary Report (ShipCode: {ShipCode}, PeriodId: {PeriodId})", shipCode, periodId);
                    return ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>.NotFound(
                        message: "Report Summary no data."
                    );
                }
                return ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>.Success(
                    data: reportList
                );

            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error retrieving Financial Summary Report (ShipCode: {ShipCode}, PeriodId: {PeriodId})", shipCode, periodId);
                return ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving Financial Summary Report (ShipCode: {ShipCode}, PeriodId: {PeriodId})", shipCode, periodId);
                return ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }
        public async Task<ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>> GetFinancialDetailReportAsync(
            string shipCode, string periodId)
        {
            var parameters = new[]
            {
                new SqlParameter("@ShipCode", shipCode),
                new SqlParameter("@PeriodId", periodId)
            };
            string sql = "EXEC SP_FinancialExpenseDetailReport @ShipCode, @PeriodId";
            try
            {
                var reportList = await _context.Set<FinancialReportDetailReadModel>()
                                 .FromSqlRaw(sql, parameters)
                                 .AsNoTracking()
                                 .ToListAsync();
                if (reportList == null || reportList.Count == 0)
                {
                    _logger.LogWarning("No data found for Financial Detail Report (ShipCode: {ShipCode}, PeriodId: {PeriodId})", shipCode, periodId);
                    return ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>.NotFound(
                        message: "Report Summary no data."
                    );
                }
                return ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>.Success(
                    data: reportList
                );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error retrieving Financial Detail Report (ShipCode: {ShipCode}, PeriodId: {PeriodId})", shipCode, periodId);
                return ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>.Failure(
                    message: $"Database error during operation: {ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving Financial Detail Report (ShipCode: {ShipCode}, PeriodId: {PeriodId})", shipCode, periodId);
                return ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>.Failure(
                    message: $"An unexpected system error occurred during operation.{ex.Message.TruncateMessage()}",
                    status: "InternalError"
                );
            }
        }
    }
}