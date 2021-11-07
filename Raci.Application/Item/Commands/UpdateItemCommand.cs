using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Raci.Application.Item.Queries.ItemDetailsGetByIdQuery;

namespace Raci.Application.Item.Commands
{
    public class UpdateItemCommand : IRequest
    {
        public ItemDetailDto Item { get; set; }

        public Guid AccountId { get; set; }

        public class Validator : AbstractValidator<UpdateItemCommand>
        {
            public Validator()
            {
                RuleFor(p => p.Item.NameVN).NotNull().NotEmpty();
                RuleFor(p => p.Item.PriceVND).NotNull().NotEqual(0);
            }
        }

        public class Handler : IRequestHandler<UpdateItemCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var item = await _context.Items
                        .Where(p => p.Id == command.Item.Id
                            && p.AuditStatus != AuditStatusEnum.Deleted
                            && p.AuditStatus != AuditStatusEnum.Undefined)
                        .SingleOrDefaultAsync();

                    if (item == null)
                    {
                        throw new ApplicationException($"Sản phẩm Id {command.Item.Id} không tồn tại.");
                    }

                    item.NameVN = command.Item.NameVN;
                    item.NameEN = command.Item.NameEN;
                    item.PriceVND = command.Item.PriceVND;
                    item.PriceUSD = command.Item.PriceUSD;
                    item.Size = command.Item.Size;
                    item.Description = command.Item.Description;

                    item.UpdatedBy = command.AccountId;
                    item.UpdatedDate = DateTime.UtcNow;

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
