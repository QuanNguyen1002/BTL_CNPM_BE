using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Entities
{
    public class Member : FullAuditedEntity<long>
    {
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }
        public long TeamId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(TeamRoleId))]
        public TeamRole TeamRole { get; set; }
        public long TeamRoleId { get; set; }
    }
}
