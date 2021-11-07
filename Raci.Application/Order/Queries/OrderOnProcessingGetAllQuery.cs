using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Domain.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Order.Queries
{
    public class OrderOnProcessingGetAllQuery : IRequest<List<OrderOnProcessingGetAllQuery.OrderByShopDto>>
    {
        public class Handler : IRequestHandler<OrderOnProcessingGetAllQuery, List<OrderByShopDto>>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<List<OrderByShopDto>> Handle(OrderOnProcessingGetAllQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var response = await _context.Shops
                        .Include(p => p.Orders)
                        .Where(p => p.AuditStatus == AuditStatusEnum.Active)
                        .Select(p => new OrderByShopDto
                        {
                            ShopId = p.Id,
                            ShopName = p.Name,
                            OrdersProccessing = p.Orders
                            .Where(od => od.OrderStatus == OrderStatusEnum.Created)
                            .Select(p => new OrderProcessingDto
                            {
                                Id = p.Id,
                                CreatedDate = p.CreatedDate,
                                CustomerName = p.Account.FullName,
                                TotalPay = p.TotalPay
                            })
                        }).ToListAsync();


                    return response;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập thông tin");
                }
            }
        }

        public class OrderByShopDto
        {
            public Guid ShopId { get; set; }

            public string ShopName { get; set; }

            public IEnumerable<OrderProcessingDto> OrdersProccessing { get; set; } = new List<OrderProcessingDto>();
        }

        public class OrderProcessingDto
        {
            public Guid Id { get; set; }

            public DateTime CreatedDate { get; set; }

            public string CustomerName { get; set; } = string.Empty;

            public double TotalPay { get; set; }
        }
    }
}
