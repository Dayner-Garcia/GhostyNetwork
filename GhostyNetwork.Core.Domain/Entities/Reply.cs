using GhostyNetwork.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Domain.Entities
{
    public class Reply : AuditableBaseEntity
    {
        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
