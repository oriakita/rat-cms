using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Item
{
    public partial class ItemDirectory
    {
        [Inject]
        private IMediator _mediator { get; set; }

        [Inject]
        private RaciDbContext _context { get; set; }

        IEnumerable<ItemDto> items;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                items = await FetchDataDirectory();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<List<ItemDto>> FetchDataDirectory()
        {
            var items = await _context.Items
                .Where(p => p.AuditStatus != AuditStatusEnum.Deleted && p.AuditStatus != AuditStatusEnum.Undefined)
                .Select(p => new ItemDto
                {
                    Id = p.Id,
                    NameVN = p.NameVN,
                    NameEN = p.NameEN,
                    PriceVND = p.PriceVND,
                    PriceUSD = p.PriceUSD,
                    Size = p.Size,
                    NumberOfOrders = p.NumberOfOrders,
                    AuditStatus = p.AuditStatus
                }).ToListAsync();

            return items;
        }
    }
}
