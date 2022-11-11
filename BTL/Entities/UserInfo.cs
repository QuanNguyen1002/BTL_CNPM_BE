using System.ComponentModel.DataAnnotations.Schema;
using System;
using Abp.Domain.Entities.Auditing;

namespace BTL.Entities
{
    public class UserInfo: FullAuditedEntity<long>
    {
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string AvataPath { get; set; }
        public SEX Sex { get; set; }
    }
    public enum SEX : byte
    {
        Nu = 0,
        Nam = 1
    }
}
