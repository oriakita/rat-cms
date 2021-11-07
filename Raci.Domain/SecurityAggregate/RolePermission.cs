using System;

namespace Raci.Domain.SecurityAggregate
{
    public class RolePermission : BaseEntity
    {
        public Guid RoleId { get; set; }

        public Guid FunctionId { get; set; }

        public long ActionId { get; set; }

        public DateTime AssignedDate { get; set; }

        public Guid AssignedBy { get; set; }

        public Function Function { get; set; }
        public Role Role { get; set; }
    }
}
