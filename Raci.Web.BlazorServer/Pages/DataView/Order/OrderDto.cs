using Raci.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public double CashAdvance { get; set; }
        public double TotalPay { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ShopName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
