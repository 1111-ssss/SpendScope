using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Table("profiles")]
    public class Profile
    {
        [Key]
        public int UserId { get; set; }
        [StringLength(20)]
        public string? DisplayName { get; set; }
        [StringLength(255)]
        public string? AvatarUrl { get; set; }
        [StringLength(400)]
        public string? Bio { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime LastOnline { get; set; }
    }
}