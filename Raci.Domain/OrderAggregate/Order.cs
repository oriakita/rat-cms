using Raci.Domain.AccountAggregate;
using Raci.Domain.Enums;
using Raci.Domain.RaciAccountAggregate;
using Raci.Domain.ShopAggregate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Raci.Domain.OrderAggregate
{
    public class Order : BaseEntity
    {
        public double TotalPrice { get; set; }
        public double TotalPay { get; set; }
        public double CashAdvance { get; set; }
        public double Change { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }

        [Range(0, 5)]
        public double CustomerRatingStar { get; set; }
        public string Note { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        public Guid RaciAccountGuid { get; set; }
        public RaciAccount RaciAccount { get; set; }

        public Guid AccountGuid { get; set; }
        public Account Account { get; set; }

        public Guid ShopGuid { get; set; }
        public Shop Shop { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
