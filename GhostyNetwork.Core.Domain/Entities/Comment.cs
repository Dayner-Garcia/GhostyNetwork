using GhostyNetwork.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Domain.Entities
{
    public class Comment : AuditableBaseEntity
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Content { get; set; }
        public List<Reply> Replies { get; set; } = new List<Reply>();
    }
}
