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
    public class CancelOrderCommand : IRequest
    {
        public Guid OrderId { get; set; }

        public Guid AccountId { get; set; }

        public class Validator : AbstractValidator<CancelOrderCommand>
        {
            public Validator() { }
        }

        public class Handler : IRequestHandler<CancelOrderCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var order = await _context.Orders
                        .Where(p => p.Id == command.OrderId
                            && p.OrderStatus == OrderStatusEnum.Created)
                        .SingleOrDefaultAsync();

                    if (order == null)
                    {
                        throw new ApplicationException($"Đơn hàng Id {command.OrderId} không tồn tại hoặc đã xử lý rồi.");
                    }

                    order.OrderStatus = OrderStatusEnum.Cancel;

                    order.UpdatedBy = command.AccountId;
                    order.UpdatedDate = DateTime.UtcNow;

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
