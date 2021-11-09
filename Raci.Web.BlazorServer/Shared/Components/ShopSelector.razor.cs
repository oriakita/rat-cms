using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Shared.Components
{
    public partial class ShopSelector
    {

        [Inject]
        private RaciDbContext _context { get; set; }

        [Parameter]
        public EventCallback<Guid> ValueChanged { get; set; }

        [Parameter]
        public Guid? Value { get; set; }

        private List<ShopTinyDto> _shop = new List<ShopTinyDto>();

        private ShopTinyDto _selectedShop = new ShopTinyDto();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _shop = await _context.Shops
                    .Where(p => p.AuditStatus == AuditStatusEnum.Active)
                    .Select(p => new ShopTinyDto
                    {
                        Id = p.Id,
                        ShopName = p.Name,
                        Address = p.Address
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected override void OnParametersSet()
        {
            _selectedShop = _shop.SingleOrDefault(p => p.Id == Value);
        }

        private async Task OnChange(object value)
        {
            _selectedShop = _shop.SingleOrDefault(p => p.Id == Guid.Parse(value.ToString()));

            await ValueChanged.InvokeAsync(_selectedShop.Id);
        }

        public class ShopTinyDto
        {
            public Guid Id { get; set; }
            public string ShopName { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
        }
    }
}
