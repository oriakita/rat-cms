using Raci.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Common.Models
{
    public class RoleModel : BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public StatusEnum Status { get; set; }

        public long? ActionId { get; set; }

        public List<UserAssignmentModel> UserAssignments { get; set; }
    }
}
