using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AEChallenge.Application.DTOs.Request
{
    public class ShipAssignmentDto
    {

        private const int MaxShipCodeLength = 50;


        public int UserId { get; set; } = 0; // Menggunakan 0 sebagai default integer
        public string ShipCode { get; set; } = string.Empty;
        public (bool IsValid, string ErrorMessage) Validate()
        {


            if (UserId <= 0)
            {
                return (false, "User ID is required and must be a valid positive number.");
            }


            if (string.IsNullOrWhiteSpace(ShipCode))
            {
                return (false, "Ship Code is required.");
            }
            if (ShipCode.Length > MaxShipCodeLength)
            {
                return (false, $"Ship Code cannot exceed {MaxShipCodeLength} characters.");
            }

            return (true, string.Empty);
        }
    }
}
