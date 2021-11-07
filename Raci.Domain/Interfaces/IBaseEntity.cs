using System;

namespace Raci.Domain.Interfaces
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
