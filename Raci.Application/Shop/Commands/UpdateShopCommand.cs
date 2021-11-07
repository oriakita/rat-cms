using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Raci.Application.Shop.Queries.ShopDetailsGetByIdQuery;

namespace Raci.Application.Shop.Commands
{
    public class UpdateShopCommand : IRequest
    {
        public ShopDetailDto Shop { get; set; }

        public Guid AccountId { get; set; }

        public class Validator : AbstractValidator<UpdateShopCommand>
        {
            public Validator()
            {
                RuleFor(p => p.Shop.Name).NotNull().NotEmpty();
                RuleFor(p => p.Shop.Address).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<UpdateShopCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateShopCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var shop = await _context.Shops
                        .Where(p => p.Id == command.Shop.Id
                            && p.AuditStatus != AuditStatusEnum.Deleted
                            && p.AuditStatus != AuditStatusEnum.Undefined)
                        .SingleOrDefaultAsync();

                    if (shop == null)
                    {
                        throw new ApplicationException($"Shop Id {command.Shop.Id} không tồn tại.");
                    }

                    shop.Name = command.Shop.Name;
                    shop.Address = command.Shop.Address;
                    shop.ReleaseDate = command.Shop.ReleaseDate;

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
