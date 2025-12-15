using System.ComponentModel.DataAnnotations;

namespace AEChallenge.Application.DTOs.Request
{
    public class CrewListParameters : BaseQueryParameters
    {
        public string ShipCode { get; set; } = string.Empty;

        public override (bool IsValid, string ErrorMessage) Validate()
        {
            var baseValidation = base.Validate();
            if (!baseValidation.IsValid)
            {
                return baseValidation;
            }
            if (string.IsNullOrEmpty(ShipCode))
            {
                return (false, $"ShipCode is required");
            }

            return (true, string.Empty);
        }
    }
}