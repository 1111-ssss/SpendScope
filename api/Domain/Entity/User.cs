using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;
        [Required]
        [StringLength(255)]
        public string Email { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }
        public bool Deleted { get; set; }
        public bool IsAdmin { get; set; }
    }
}