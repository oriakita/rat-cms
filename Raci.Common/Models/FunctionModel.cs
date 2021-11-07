using Raci.Common.Enums;
using System;
using System.Collections.Generic;

namespace Raci.Common.Models
{
    public class FunctionModel : BaseModel
    {
        public string Code { get; set; }

        public Guid ModuleId { get; set; }

        public string Name { get; set; }
        public int OrderNumber { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }

        public string Description { get; set; }

        public StatusEnum Status { get; set; }

        public virtual ModuleModel Module { get; set; }
        public virtual IEnumerable<ActionModel> Actions { get; set; }
    }
}
