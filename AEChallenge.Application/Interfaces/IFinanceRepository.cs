using AEChallenge.Application.DTOs.Response;

namespace AEChallenge.Application.Interfaces
{
    public interface IFinanceRepository
    {
        Task<ApiResponseResult<IEnumerable<FinancialReportSummaryReadModel>>> GetFinancialSummaryReportAsync(
             string shipCode,
             string periodId
         );
        Task<ApiResponseResult<IEnumerable<FinancialReportDetailReadModel>>> GetFinancialDetailReportAsync(
            string shipCode,
            string periodId
        );
    }
}