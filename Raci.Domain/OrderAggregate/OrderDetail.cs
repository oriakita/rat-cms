using Raci.Domain.ItemAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Domain.OrderAggregate
{
    public class OrderDetail : BaseEntity
    {
        public Guid ItemGuid { get; set; }
        public Item Item { get; set; }
        public Guid OrderGuid { get; set; }
        public Order Order { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
