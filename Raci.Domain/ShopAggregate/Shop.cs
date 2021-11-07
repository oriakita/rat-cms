using Raci.Common.Enums;
using Raci.Domain.OrderAggregate;
using Raci.Domain.RaciAccountAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Domain.ShopAggregate
{
    public class Shop : BaseEntity
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }

        public AuditStatusEnum AuditStatus { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<RaciAccount> Staffs { get; set; }
    }
}
