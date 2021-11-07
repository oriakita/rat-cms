using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Domain.Enums;
using Raci.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Item.Queries
{
    public class ItemDetailsGetByIdQuery : IRequest<ItemDetailsGetByIdQuery.ItemDetailDto>
    {
        public Guid ItemId { get; set; }

        public class Handler : IRequestHandler<ItemDetailsGetByIdQuery, ItemDetailDto>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<ItemDetailDto> Handle(ItemDetailsGetByIdQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var item = await _context.Items.AsNoTracking()
                        .Where(p => p.Id == query.ItemId
                        && p.AuditStatus != AuditStatusEnum.Deleted
                        && p.AuditStatus != AuditStatusEnum.Undefined)
                        .SingleOrDefaultAsync();

                    if (item == null)
                    {
                        throw new ApplicationException($"ItemId {query.ItemId} không tồn tại.");
                    }

                    var response = new ItemDetailDto
                    {
                        Id = item.Id,
                        NameVN = item.NameVN,
                        NameEN = item.NameEN,
                        PriceVND = item.PriceVND,
                        PriceUSD = item.PriceUSD,
                        Size = item.Size,
                        Description = item.Description,
                        ImageUrl = item.ImageUrl,
                        ItemRatingStar = item.ItemRatingStar,
                        NumberOfOrders = item.NumberOfOrders,
                        AuditStatus = item.AuditStatus
                    };

                    return response;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập thông tin");
                }
            }
        }

        public class ItemDetailDto
        {
            public Guid Id { get; set; }

            public string NameVN { get; set; } = string.Empty;

            public string NameEN { get; set; } = string.Empty;

            public double PriceVND { get; set; }

            public double PriceUSD { get; set; }

            public ItemSizeEnum Size { get; set; }

            public string Description { get; set; } = string.Empty;

            public string ImageUrl { get; set; } = string.Empty;

            public double ItemRatingStar { get; set; }

            public int NumberOfOrders { get; set; }

            public Guid CreatedBy { get; set; }

            public Guid UpdatedBy { get; set; }

            public AuditStatusEnum AuditStatus { get; set; }
        }
    }
}
