using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Shop
{
    public partial class ShopDirectory
    {
        [Inject]
        private IMediator _mediator { get; set; }

        [Inject]
        private RaciDbContext _context { get; set; }

        IEnumerable<ShopDto> shops;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                shops = await FetchDataDirectory();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<List<ShopDto>> FetchDataDirectory()
        {
            var shops = await _context.Shops
                .Where(p => p.AuditStatus != AuditStatusEnum.Deleted && p.AuditStatus != AuditStatusEnum.Undefined)
                .Select(p => new ShopDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    ReleaseDate = p.ReleaseDate,
                    AuditStatus = p.AuditStatus
                }).ToListAsync();

            return shops;
        }
    }
}
