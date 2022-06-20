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
    public class GetOrderReportByShopAndByMonthQuery : IRequest<List<GetOrderReportByShopAndByMonthQuery.DataOrderReportInDayDto>>
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public Guid ShopId { get; set; }

        public class Handler : IRequestHandler<GetOrderReportByShopAndByMonthQuery, List<DataOrderReportInDayDto>>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<List<DataOrderReportInDayDto>> Handle(GetOrderReportByShopAndByMonthQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var orders = await _context.Orders
                        .Include(p => p.OrderDetails)
                        .AsNoTracking()
                        .Where(p => query.FromDate.AddHours(-7) <= p.UpdatedDate
                            && p.UpdatedDate <= query.ToDate.AddHours(-7)
                            && p.ShopGuid == query.ShopId)
                        .ToListAsync();

                    var results = new List<DataOrderReportInDayDto>();

                    int days = DateTime.DaysInMonth(query.FromDate.Year, query.FromDate.Month);
                    for (int day = 1; day <= days; day++)
                    {
                        var startOfDay = new DateTime(query.FromDate.Year, query.FromDate.Month, day);
                        var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

                        results.Add(new DataOrderReportInDayDto
                        {
                            DayName = $"Ng {day}",
                            TotalRevenue = orders
                                .Where(p => startOfDay.AddHours(-7) <= p.UpdatedDate
                                    && p.UpdatedDate <= endOfDay.AddHours(-7))
                                .ToList()
                                .Sum(p => p.TotalPay)
                        });
                    }

                    return results;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập thông tin");
                }
            }
        }

        public class DataOrderReportInDayDto
        {
            public string DayName { get; set; } = string.Empty;

            public double TotalRevenue { get; set; }
        }
    }
}
