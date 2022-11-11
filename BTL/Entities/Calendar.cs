using BTL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Abp.Domain.Entities.Auditing;

namespace BTL.Entities
{
    public class Calendar : FullAuditedEntity<long>
    {
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }
        public long TeamId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public CalendarType CalendarType { get; set; }
    }
    public enum CalendarType : byte
    {
        None  = 0,
    }
}
