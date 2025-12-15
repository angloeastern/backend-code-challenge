using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AEChallenge.Application.DTOs.Request
{
    public partial class FinancialReportParameters
    {
        public string? ShipCode { get; set; }
        public string? PeriodId { get; set; }

        private static readonly Regex PeriodIdRegex =
            MyRegex();

        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(ShipCode))
            {
                return (false, "ShipCode is required.");
            }

            if (string.IsNullOrWhiteSpace(PeriodId))
            {
                return (false, "PeriodId is required.");
            }

            if (!PeriodIdRegex.IsMatch(PeriodId))
            {
                return (false, "PeriodId must be in YYYYMM format (e.g., 202402).");
            }

            if (!int.TryParse(PeriodId, out _))
            {
                return (false, "PeriodId must be a valid numerical date.");
            }

            string monthString = PeriodId.Substring(4, 2);
            if (int.TryParse(monthString, out int month) && (month < 1 || month > 12))
            {
                return (false, "The month part of PeriodId must be between 01 and 12.");
            }
            return (true, string.Empty);
        }

        [GeneratedRegex(@"^\d{6}$", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
    }
}
