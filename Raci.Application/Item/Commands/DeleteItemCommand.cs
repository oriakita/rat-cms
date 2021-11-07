using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Item.Commands
{
    public class DeleteItemCommand : IRequest
    {
        public Guid ItemId { get; set; }

        public Guid AccountId { get; set; }

        public class Validator : AbstractValidator<DeleteItemCommand>
        {
            public Validator() { }
        }

        public class Handler : IRequestHandler<DeleteItemCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var item = await _context.Items
                        .Where(p => p.Id == command.ItemId
                            && p.AuditStatus != AuditStatusEnum.Deleted
                            && p.AuditStatus != AuditStatusEnum.Undefined)
                        .SingleOrDefaultAsync();

                    if (item == null)
                    {
                        throw new ApplicationException($"Sản phẩm Id {command.ItemId} không tồn tại.");
                    }

                    item.AuditStatus = AuditStatusEnum.Deleted;

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
