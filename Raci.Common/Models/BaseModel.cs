using System;

namespace Raci.Common.Models
{
    public abstract class BaseModel
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual DateTime UpdatedDate { get; set; }
    }
}
