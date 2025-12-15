using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("Budget")]
    public class Budget : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BudgetID { get; set; }

        [Required]
        [StringLength(10)]
        public string ShipCode { get; set; } = string.Empty;

        [Required]
        public int COA_ID { get; set; }

        [Required]
        public int PeriodID { get; set; }

        [Required]
        [Column(TypeName = "decimal(19, 2)")]
        public decimal BudgetTarget { get; set; }


        public Ship Ship { get; set; } = default!;
        public ChartOfAccount COA { get; set; } = default!;
        public FiscalPeriod Period { get; set; } = default!;
    }
}