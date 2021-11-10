using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Shop.Queries
{
    public class GetShopSelectorQuery : IRequest<List<GetShopSelectorQuery.ShopTinyDto>>
    {

        public class Handler : IRequestHandler<GetShopSelectorQuery, List<ShopTinyDto>>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<List<ShopTinyDto>> Handle(GetShopSelectorQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var shops = await _context.Shops
                        .Where(p => p.AuditStatus == AuditStatusEnum.Active)
                        .Select(p => new ShopTinyDto
                        {
                            Id = p.Id,
                            ShopName = p.Name,
                            Address = p.Address
                        }).ToListAsync();

                    return shops;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập thông tin");
                }
            }
        }

        public class ShopTinyDto
        {
            public Guid Id { get; set; }
            public string ShopName { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
        }
    }
}
