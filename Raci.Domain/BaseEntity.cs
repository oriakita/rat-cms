using Raci.Domain.Interfaces;
using System;

namespace Raci.Domain
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
