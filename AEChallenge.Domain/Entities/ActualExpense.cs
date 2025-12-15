using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("ActualExpense")]
    public class ActualExpense : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ActualID { get; set; }

        [Required]
        [StringLength(10)]
        public string ShipCode { get; set; } = string.Empty;

        [Required]
        public int COA_ID { get; set; }

        [Required]
        public int PeriodID { get; set; }

        [Required]
        [Column(TypeName = "decimal(19, 2)")]
        public decimal ActualAmount { get; set; }


        public Ship Ship { get; set; } = default!;
        public ChartOfAccount COA { get; set; } = default!;
        public FiscalPeriod Period { get; set; } = default!;
    }
}