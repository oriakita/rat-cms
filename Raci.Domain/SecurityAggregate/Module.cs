using Raci.Domain.Enums;
using System.Collections.Generic;

namespace Raci.Domain.SecurityAggregate
{
    public class Module : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public int OrderNumber { get; set; }

        public string Description { get; set; } = string.Empty;

        public StatusEnum Status { get; set; }

        public string RootPath { get; set; } = string.Empty;

        public ICollection<Function> Functions { get; set; }
    }
}
