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
    public class ShopDetailsGetByIdQuery : IRequest<ShopDetailsGetByIdQuery.ShopDetailDto>
    {
        public Guid ShopId { get; set; }

        public class Handler : IRequestHandler<ShopDetailsGetByIdQuery, ShopDetailDto>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<ShopDetailDto> Handle(ShopDetailsGetByIdQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var shop = await _context.Shops
                        .Where(p => p.Id == query.ShopId
                        && p.AuditStatus != AuditStatusEnum.Deleted
                        && p.AuditStatus != AuditStatusEnum.Undefined)
                        .SingleOrDefaultAsync();

                    if (shop == null)
                    {
                        throw new ApplicationException($"ShopId {query.ShopId} không tồn tại.");
                    }

                    var response = new ShopDetailDto
                    {
                        Id = shop.Id,
                        Name = shop.Name,
                        Address = shop.Address,
                        ReleaseDate = shop.ReleaseDate
                    };

                    return response;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập thông tin");
                }
            }
        }

        public class ShopDetailDto
        {
            public Guid Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public string Address { get; set; } = string.Empty;

            public DateTime ReleaseDate { get; set; }
        }
    }
}
