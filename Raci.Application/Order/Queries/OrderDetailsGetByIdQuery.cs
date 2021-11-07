using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Domain.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Order.Queries
{
    public class OrderDetailsGetByIdQuery : IRequest<OrderDetailsGetByIdQuery.OrderDetailDto>
    {
        public Guid OrderId { get; set; }

        public class Handler : IRequestHandler<OrderDetailsGetByIdQuery, OrderDetailDto>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<OrderDetailDto> Handle(OrderDetailsGetByIdQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var order = await _context.Orders
                        .Include(p => p.OrderDetails).ThenInclude(detail => detail.Item)
                        .Include(p => p.RaciAccount)
                        .Include(p => p.Account)
                        .Include(p => p.Shop)
                        .Where(p => p.Id == query.OrderId)
                        .SingleOrDefaultAsync();

                    if (order == null)
                    {
                        throw new ApplicationException($"Đơn hàng Id {query.OrderId} không tồn tại.");
                    }

                    var response = new OrderDetailDto
                    {
                        Id = order.Id,
                        CashAdvance = order.CashAdvance,
                        Change = order.Change,
                        TotalPay = order.TotalPay,
                        TotalPrice = order.TotalPrice,
                        CreatedDate = order.CreatedDate,
                        OrderStatus = order.OrderStatus,

                        CustomerRatingStar = order.CustomerRatingStar,
                        Note = order.Note,

                        AccountId = order.AccountGuid,
                        CustomerName = order.Account.FullName,
                        RaciAccountId = order.RaciAccountGuid,
                        CashierName = order.RaciAccount.FirstName + " " + order.RaciAccount.LastName,
                        ShopId = order.ShopGuid,
                        ShopName = order.Shop.Name,
                        ShopAddress = order.Shop.Address
                    };

                    if (order.OrderDetails != null)
                    {
                        response.OrderItemDetails = order.OrderDetails
                            .Select(p => new OrderItemDetailDto
                            {
                                ItemId = p.ItemGuid,
                                ItemName = p.Item.NameVN,
                                Quantity = p.Quantity,
                                UnitPrice = p.UnitPrice
                            }).ToList();
                    };

                    return response;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập thông tin");
                }
            }
        }

        public class OrderDetailDto
        {
            public Guid Id { get; set; }

            public double CashAdvance { get; set; }

            public double Change { get; set; }

            public double TotalPay { get; set; }

            public double TotalPrice { get; set; }

            public OrderStatusEnum OrderStatus { get; set; }

            public double CustomerRatingStar { get; set; }

            public string Note { get; set; } = string.Empty;

            public DateTime CreatedDate { get; set; }

            public Guid RaciAccountId { get; set; }

            public string CashierName { get; set; } = string.Empty;

            public Guid AccountId { get; set; }

            public string CustomerName { get; set; } = string.Empty;

            public Guid ShopId { get; set; }

            public string ShopName { get; set; } = string.Empty;

            public string ShopAddress { get; set; } = string.Empty;

            public IEnumerable<OrderItemDetailDto> OrderItemDetails { get; set; } = new List<OrderItemDetailDto>();
        }

        public class OrderItemDetailDto
        {
            public Guid ItemId { get; set; }
            public string ItemName { get; set; } = string.Empty;
            public double UnitPrice { get; set; }
            public int Quantity { get; set; }
        }
    }
}
