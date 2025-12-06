using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Table("follows")]
    public class Follow
    {
        [Key, Column(Order = 0)]
        public int FollowerId { get; set; }
        [Key, Column(Order = 1)]
        public int FollowedId { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime FollowedAt { get; set; }
    }
}