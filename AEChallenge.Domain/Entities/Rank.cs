using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("Rank")]
    public class Rank : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RankCode { get; set; }

        [Required]
        [StringLength(50)]
        public string RankName { get; set; } = string.Empty; // Unique Index

        public ICollection<CrewServiceHistory> CrewServiceHistory { get; set; } = [];
    }
}