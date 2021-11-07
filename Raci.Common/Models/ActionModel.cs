using Raci.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raci.Common.Models
{
    public class ActionModel : BaseModel
    {
        public long ActionId { get; set; }

        public Guid FunctionId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public StatusEnum Status { get; set; }

        [NotMapped]
        public bool HasPermission { get; set; }
    }
}
