using FluentValidation;
using MediatR;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Raci.Application.Item.Queries.ItemDetailsGetByIdQuery;

namespace Raci.Application.Item.Commands
{
    public class AddItemCommand : IRequest
    {
        public Guid AccountId { get; set; }

        public ItemDetailDto Item { get; set; }

        public class Validator : AbstractValidator<AddItemCommand>
        {
            public Validator()
            {
                RuleFor(p => p.Item.NameVN).NotNull().NotEmpty();
                RuleFor(p => p.Item.PriceVND).NotNull().NotEqual(0);
            }
        }

        public class Handler : IRequestHandler<AddItemCommand>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AddItemCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var item = new Domain.ItemAggregate.Item
                    {
                        Id = command.Item.Id,
                        NameVN = command.Item.NameVN,
                        NameEN = command.Item.NameEN,
                        PriceVND = command.Item.PriceVND,
                        PriceUSD = command.Item.PriceUSD,
                        Size = command.Item.Size,
                        Description = command.Item.Description,

                        ImageUrl = @"\images\default.jpg",
                        ItemRatingStar = 0,
                        NumberOfOrders = 0,
                        AuditStatus = AuditStatusEnum.Active,
                        
                        CreatedBy = command.AccountId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    };

                    await _context.Items.AddAsync(item);

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
