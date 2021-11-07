using Raci.Domain.Enums;
using System;

namespace Raci.Domain.SecurityAggregate
{
    public class Action : BaseEntity
    {
        public long ActionId { get; set; }

        public Guid FunctionId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public StatusEnum Status { get; set; }

        public Function Function { get; set; }
    }
}
