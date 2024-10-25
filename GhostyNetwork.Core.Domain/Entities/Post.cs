using GhostyNetwork.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        public string? VideoUrl { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
