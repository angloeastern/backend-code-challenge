using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("CrewMember")]
    public class CrewMember : BaseEntity
    {
        [Key]
        [StringLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Diasumsikan ID ditetapkan secara eksternal
        public string CrewMemberId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public DateOnly BirthDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; } = string.Empty;

        public ICollection<CrewServiceHistory> ServiceHistory { get; set; } = [];
    }
}