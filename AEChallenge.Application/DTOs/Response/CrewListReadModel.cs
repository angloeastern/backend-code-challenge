using Microsoft.EntityFrameworkCore;

namespace AEChallenge.Application.DTOs.Response
{
    [Keyless]
    public class CrewListReadModel
    {
        public string CrewMemberId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string RankName { get; set; } = string.Empty;
        public string SignOnDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Nationality { get; set; } = string.Empty;
    }
}
