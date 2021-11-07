using System;

namespace Raci.Common.Models
{
    public class UserAssignmentModel : BaseModel
    {
        public Guid RaciAccountId { get; set; }

        public Guid RoleId { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime AssignedDateUtc { get; set; }

        public Guid AssignedBy { get; set; }

        public virtual RaciAccountModel User { get; set; }
    }
}
