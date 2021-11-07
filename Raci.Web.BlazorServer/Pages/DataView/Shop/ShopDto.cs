using Raci.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Shop
{
    public class ShopDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime ReleaseDate { get; set; }

        public AuditStatusEnum AuditStatus { get; set; }
    }
}
