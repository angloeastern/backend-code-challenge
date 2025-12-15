using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("ChartOfAccount")]
    public class ChartOfAccount : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int COA_ID { get; set; } // Primary Key

        [Required]
        [StringLength(10)]
        public string AccountNumber { get; set; } = string.Empty; // Unique Index

        [Required]
        [StringLength(100)]
        public string AccountDescription { get; set; } = string.Empty;


        public int? ParentCOA_ID { get; set; }
        [ForeignKey("ParentCOA_ID")]
        public ChartOfAccount? ParentAccount { get; set; }
        public ICollection<ChartOfAccount> ChildAccounts { get; set; } = [];

        [Required]
        [StringLength(10)]
        public string AccountType { get; set; } = string.Empty; // 'Child' atau 'Parent'
    }
}