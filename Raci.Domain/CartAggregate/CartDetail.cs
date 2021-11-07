using Raci.Domain.AccountAggregate;
using Raci.Domain.ItemAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Domain.CartAggregate
{
    public class CartDetail : BaseEntity
    {
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        public int Quantity { get; set; }
    }
}
