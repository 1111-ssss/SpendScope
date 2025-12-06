using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Table("app_versions")]
    public class AppVersion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Branch { get; set; } = null!;
        [Required]
        public int Build { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime UploadedAt { get; set; }
        public int? UploadedBy { get; set; }
        [StringLength(250)]
        public string Changelog { get; set; } = null!;
    }
}