using System;

namespace Raci.Common.Models
{
    public class RolePermissionDto
    {
        public Guid RoleId { get; set; }

        public Guid FunctionId { get; set; }

        public long ActionId { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime AssignedDateUtc { get; set; }

        public Guid AssignedBy { get; set; }

        public virtual FunctionModel Function { get; set; }
        public virtual RoleModel Role { get; set; }
    }
}
