using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("Ship")]
    public class Ship : BaseEntity
    {
        [Key]
        [StringLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ShipCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ShipName { get; set; } = string.Empty;

        public int FiscalYear { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; } = string.Empty; // 'Active' atau 'Inactive'


        public ICollection<ActualExpense> ActualExpenses { get; set; } = [];
        public ICollection<Budget> Budgets { get; set; } = [];
        public ICollection<CrewServiceHistory> ServiceHistory { get; set; } = [];
        public ICollection<UserShipAssignment> UserShipAssignments { get; set; } = [];
    }
}