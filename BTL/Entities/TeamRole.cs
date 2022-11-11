using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL.Entities
{
    public class TeamRole : FullAuditedEntity<long>
    {
        public string Description { get; set; }
        public string Name { get; set; }

    }
}
