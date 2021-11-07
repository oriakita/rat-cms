using Raci.Common.Enums;
using Raci.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Item
{
    public class ItemDto
    {
        public Guid Id { get; set; }

        public string NameVN { get; set; } = string.Empty;

        public string NameEN { get; set; } = string.Empty;

        public double PriceVND { get; set; }

        public double PriceUSD { get; set; }

        public ItemSizeEnum Size { get; set; }

        public int NumberOfOrders { get; set; }

        public AuditStatusEnum AuditStatus { get; set; }
    }
}
