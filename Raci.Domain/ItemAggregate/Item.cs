using Raci.Common.Enums;
using Raci.Domain.CartAggregate;
using Raci.Domain.Enums;
using Raci.Domain.OrderAggregate;
using System;
using System.Collections.Generic;

namespace Raci.Domain.ItemAggregate
{
    public class Item : BaseEntity
    {
        public string NameVN { get; set; }

        public string NameEN { get; set; }

        public double PriceVND { get; set; }

        public double PriceUSD { get; set; }

        public ItemSizeEnum Size { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public double ItemRatingStar { get; set; }

        public int NumberOfOrders { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }

        public AuditStatusEnum AuditStatus { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }

        public ICollection<CartDetail>? CartDetails { get; set; }
    }
}
