
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("CrewServiceHistory")]
    public class CrewServiceHistory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ServiceHistoryId { get; set; }

        [Required]
        [StringLength(10)]
        public string CrewMemberId { get; set; } = string.Empty;
        public CrewMember CrewMember { get; set; } = default!;

        [Required]
        [StringLength(10)]
        public string ShipCode { get; set; } = string.Empty;
        public Ship Ship { get; set; } = default!;

        public int RankCode { get; set; }
        public Rank Rank { get; set; } = default!;

        public DateOnly SignOnDate { get; set; }
        public DateOnly? SignOffDate { get; set; } // Bisa Null
        public DateOnly EndOfContractDate { get; set; }
    }
}