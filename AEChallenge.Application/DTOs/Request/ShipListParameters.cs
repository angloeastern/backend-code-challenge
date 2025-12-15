using System.ComponentModel.DataAnnotations;

namespace AEChallenge.Application.DTOs.Request
{
    public class ShipListParameters : BaseQueryParameters
    {
        public string Status { get; set; } = string.Empty;

        public override (bool IsValid, string ErrorMessage) Validate()
        {
            var baseValidation = base.Validate();
            if (!baseValidation.IsValid)
            {
                return baseValidation;
            }
            return (true, string.Empty);
        }
    }
}