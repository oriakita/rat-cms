using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Domain.Enums;
using Raci.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Order.Commands
{
    public class PaidOrderCommand : IRequest
    {
        public Guid OrderId { get; set; }

        public Guid AccountId { get; set; }

        public double CashAdvance { get; set; }

        public double Change { get; set; }

        public class Validator : AbstractValidator<PaidOrderCommand>
        {
            public Validator() { }
        }

        public class Handler : IRequestHandler<PaidOrderCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(PaidOrderCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var order = await _context.Orders
                        .Include(p => p.OrderDetails)
                        .ThenInclude(p => p.Item)
                        .Where(p => p.Id == command.OrderId
                            && p.OrderStatus == OrderStatusEnum.Created)
                        .SingleOrDefaultAsync();

                    if (order == null)
                    {
                        throw new ApplicationException($"Đơn hàng Id {command.OrderId} không tồn tại hoặc đã xử lý rồi.");
                    }

                    order.CashAdvance = command.CashAdvance;
                    order.Change = command.Change;
                    order.OrderStatus = OrderStatusEnum.Paid;

                    order.UpdatedBy = command.AccountId;
                    order.UpdatedDate = DateTime.UtcNow;

                    #region Update Soldered number of item

                    foreach(var detail in order.OrderDetails)
                    {
                        detail.Item.NumberOfOrders = detail.Item.NumberOfOrders + detail.Quantity;
                    }

                    #endregion

                    await _context.SaveChangesAsync();

                    return Unit.Value;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
