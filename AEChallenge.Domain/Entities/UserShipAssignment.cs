using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("UserShipAssignment")]
    public class UserShipAssignment : BaseEntity
    {

        public int UserId { get; set; }


        [StringLength(10)]
        public string ShipCode { get; set; } = string.Empty;


        public SystemUser User { get; set; } = default!;
        public Ship Ship { get; set; } = default!;
    }
}