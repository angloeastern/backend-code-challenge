using Microsoft.EntityFrameworkCore;

namespace AEChallenge.Application.DTOs.Response
{
    [Keyless]
    public class ShipAssignmentReadModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ShipCode { get; set; } = string.Empty;
        public string ShipName { get; set; } = string.Empty;
        public int FiscalYear { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public string AssignedByUsername { get; set; } = string.Empty;
    }
}
