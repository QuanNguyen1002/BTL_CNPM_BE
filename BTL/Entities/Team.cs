using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL.Entities
{
    public class Team: FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public TeamType TeamType { get; set; }
    }
    public enum TeamType: byte
    {
        OnlyMe = 1,
        Class = 2,
        Office = 3,
        Custom = 4
    }
}
