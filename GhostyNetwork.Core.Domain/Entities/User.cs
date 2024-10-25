using GhostyNetwork.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }

        [MaxLength(1000)]
        public string ProfilePicture { get; set; }

        public bool IsActive { get; set; }
        public string ActivationToken { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Reply> Replies { get; set; } = new List<Reply>();
    }
}
