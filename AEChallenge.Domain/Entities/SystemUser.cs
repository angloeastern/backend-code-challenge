using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEChallenge.Domain.Entities
{
    [Table("SystemUser")]
    public class SystemUser : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty; // Unique Index

        [Required]
        [StringLength(50)]
        public string UserRole { get; set; } = string.Empty;

        [Column(TypeName = "varbinary(128)")]
        public byte[]? PasswordHash { get; set; } // Menggunakan byte[] untuk varbinary

        public ICollection<UserShipAssignment> Assignments { get; set; } = [];
    }
}