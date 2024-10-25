using GhostyNetwork.Core.Domain.Common;

namespace GhostyNetwork.Core.Domain.Entities
{
    public class Friendship : AuditableBaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int FriendId { get; set; }
        public User Friend { get; set; }
    }
}
