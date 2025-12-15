using Microsoft.EntityFrameworkCore;

namespace AEChallenge.Application.DTOs.Response
{
    [Keyless]
    public class FinancialReportDetailReadModel : FinancialReportSummaryReadModel
    {
        public string ParentAccountNumber { get; set; } = string.Empty;
        public string ParentAccountDescription { get; set; } = string.Empty;

    }
}