using Raci.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Common.Models
{
    public class ModuleModel : BaseModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int OrderNumber { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; } = StatusEnum.Active;

        public string RootPath { get; set; }
    }
}
