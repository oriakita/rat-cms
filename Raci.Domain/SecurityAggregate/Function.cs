using Raci.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Raci.Domain.SecurityAggregate
{
    public class Function : BaseEntity
    {
        public Guid ModuleId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int OrderNumber { get; set; }

        public string Url { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public StatusEnum Status { get; set; }

        public Module Module { get; set; }
        public ICollection<Action> Actions { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
