using FluentValidation;
using MediatR;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Raci.Application.Shop.Queries.ShopDetailsGetByIdQuery;

namespace Raci.Application.Shop.Commands
{
    public class AddShopCommand : IRequest
    {
        public Guid AccountId { get; set; }

        public ShopDetailDto Shop { get; set; }

        public class Validator : AbstractValidator<AddShopCommand>
        {
            public Validator()
            {
                RuleFor(p => p.Shop.Name).NotNull().NotEmpty();
                RuleFor(p => p.Shop.Address).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<AddShopCommand>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AddShopCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var shop = new Domain.ShopAggregate.Shop
                    {
                        Id = command.Shop.Id,
                        Name = command.Shop.Name,
                        Address = command.Shop.Address,
                        ReleaseDate = command.Shop.ReleaseDate,

                        CreatedBy = command.AccountId,
                        AuditStatus = AuditStatusEnum.Active,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    };

                    await _context.Shops.AddAsync(shop);

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
