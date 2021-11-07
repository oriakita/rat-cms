using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Shop.Commands
{
    public class DeleteShopCommand : IRequest
    {
        public Guid ShopId { get; set; }

        public Guid AccountId { get; set; }

        public class Validator : AbstractValidator<DeleteShopCommand>
        {
            public Validator() { }
        }

        public class Handler : IRequestHandler<DeleteShopCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteShopCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var shop = await _context.Shops
                        .Where(p => p.Id == command.ShopId
                            && p.AuditStatus != AuditStatusEnum.Deleted
                            && p.AuditStatus != AuditStatusEnum.Undefined)
                        .SingleOrDefaultAsync();

                    if (shop == null)
                    {
                        throw new ApplicationException($"Shop Id {command.ShopId} không tồn tại.");
                    }

                    shop.AuditStatus = AuditStatusEnum.Deleted;

                    shop.UpdatedBy = command.AccountId;
                    shop.UpdatedDate = DateTime.UtcNow;

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
