using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("FiscalPeriod")]
    public class FiscalPeriod : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // PeriodID adalah PK, tapi tidak auto-increment
        public int PeriodID { get; set; }
        public int FiscalYear { get; set; }
        public int PeriodMonth { get; set; }
        public DateOnly StartDate { get; set; } // Menggunakan DateOnly (Net 6+)
        public DateOnly EndDate { get; set; }


        public ICollection<Budget> Budgets { get; set; } = [];
        public ICollection<ActualExpense> ActualExpenses { get; set; } = [];
    }
}