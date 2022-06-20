using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Order.Queries
{
    public class GetOrderReportByShopAndByDateQuery : IRequest<GetOrderReportByShopAndByDateQuery.OrderReportDto>
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public Guid ShopId { get; set; }

        public class Handler : IRequestHandler<GetOrderReportByShopAndByDateQuery, OrderReportDto>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<OrderReportDto> Handle(GetOrderReportByShopAndByDateQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var orders = await _context.Orders
                        .Include(p => p.OrderDetails)
                        .Where(p => query.FromDate.AddHours(-7) <= p.UpdatedDate
                            && p.UpdatedDate <= query.ToDate.AddHours(-7)
                            && p.ShopGuid == query.ShopId)
                        .ToListAsync();

                    var response = new OrderReportDto()
                    {
                        TotalRevenue = orders.Sum(p => p.TotalPay)
                    };

                    foreach (var order in orders)
                    {
                        response.TotalItems = response.TotalItems + order.OrderDetails.Sum(p => p.Quantity);
                    }

                    return response;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập thông tin");
                }
            }
        }

        public class OrderReportDto
        {
            public double TotalRevenue { get; set; }

            public int TotalItems { get; set; }
        }
    }
}
