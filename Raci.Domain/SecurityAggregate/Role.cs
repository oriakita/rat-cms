using Raci.Domain.Enums;
using System.Collections.Generic;

namespace Raci.Domain.SecurityAggregate
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public StatusEnum Status { get; set; }

        public ICollection<UserAssignment> UserAssignments { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
