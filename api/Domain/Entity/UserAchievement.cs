using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    [Table("user_achievements")]
    public class UserAchievement
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int AchievementId { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime UnlockedAt { get; set; }
    }
}