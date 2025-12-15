using System.Text.RegularExpressions;

namespace AEChallenge.Application.DTOs.Request
{
    public partial class ShipCreateDto
    {

        private const int MinShipCodeLength = 3;
        private const int MaxShipCodeLength = 50;
        private const int MaxShipNameLength = 150;
        private const int MaxStatusLength = 50; // Max length untuk kolom Status di DB


        private const int MinFiscalYear = 2000;
        private const int MaxFiscalYear = 2100;


        public const string StatusActive = "Active";
        public const string StatusInactive = "Inactive";


        private static readonly List<string> ValidStatuses =
    [
        StatusActive,
        StatusInactive
    ];

        public string ShipCode { get; set; } = string.Empty;
        public string ShipName { get; set; } = string.Empty;
        public int FiscalYear { get; set; } = 0;
        public string Status { get; set; } = StatusActive; // Default Active



        public (bool IsValid, string ErrorMessage) Validate()
        {

            if (string.IsNullOrWhiteSpace(ShipCode))
            {
                return (false, "Ship Code is required.");
            }
            if (ShipCode.Length < MinShipCodeLength || ShipCode.Length > MaxShipCodeLength)
            {
                return (false, $"Ship Code must be between {MinShipCodeLength} and {MaxShipCodeLength} characters.");
            }
            if (!MyRegex().IsMatch(ShipCode))
            {
                return (false, "Ship Code can only contain alphanumeric characters, dash, or underscore.");
            }


            if (string.IsNullOrWhiteSpace(ShipName))
            {
                return (false, "Ship Name is required.");
            }
            if (ShipName.Length > MaxShipNameLength)
            {
                return (false, $"Ship Name cannot exceed {MaxShipNameLength} characters.");
            }


            if (FiscalYear < MinFiscalYear || FiscalYear > MaxFiscalYear)
            {
                return (false, $"Fiscal Year must be a valid year, typically between {MinFiscalYear} and {MaxFiscalYear}.");
            }


            if (string.IsNullOrWhiteSpace(Status))
            {
                return (false, "Status is required.");
            }

            if (!ValidStatuses.Contains(Status, StringComparer.OrdinalIgnoreCase))
            {
                string allowedStatuses = string.Join(" or ", ValidStatuses);
                return (false, $"Invalid Status value. Allowed values are: {allowedStatuses}.");
            }

            return (true, string.Empty);
        }

        [GeneratedRegex(@"^[a-zA-Z0-9_-]+$")]
        private static partial Regex MyRegex();
    }
}
