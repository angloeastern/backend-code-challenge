using Microsoft.EntityFrameworkCore;

namespace AEChallenge.Application.DTOs.Response
{
    [Keyless]
    public class FinancialReportSummaryReadModel
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountDescription { get; set; } = string.Empty;
        public decimal CurrentActual { get; set; } = 0M;
        public decimal CurrentBudget { get; set; } = 0M;
        public decimal CurrentVariance { get; set; } = 0M;
        public decimal CurrentVariancePercent { get; set; } = 0M;

        public decimal YTDActual { get; set; } = 0M;
        public decimal YTDBudget { get; set; } = 0M;
        public decimal YTDVariance { get; set; } = 0M;
        public decimal YTDVariancePercent { get; set; } = 0M;
        public string StartPeriodIDName { get; set; } = string.Empty;
        public string CurrentPeriodIDName { get; set; } = string.Empty;

    }

}