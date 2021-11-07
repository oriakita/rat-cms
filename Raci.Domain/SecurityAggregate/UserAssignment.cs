using Raci.Domain.RaciAccountAggregate;
using System;

namespace Raci.Domain.SecurityAggregate
{
    public class UserAssignment : BaseEntity
    {
        public Guid RaciAccountId { get; set; }

        public Guid RoleId { get; set; }

        public DateTime AssignedDate { get; set; }

        public Guid AssignedBy { get; set; }

        public RaciAccount RaciAccount { get; set; }
        public Role Role { get; set; }
    }
}
