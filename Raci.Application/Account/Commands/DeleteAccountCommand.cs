using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Application.Account.Queries;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Account.Commands
{
    public class DeleteAccountCommand : IRequest
    {
        public Guid AccountId { get; set; }

        public class Validator : AbstractValidator<DeleteAccountCommand>
        {
            public Validator() { }
        }

        public class Handler : IRequestHandler<DeleteAccountCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var account = await _context.RaciAccounts
                        .Where(p => p.Id == command.AccountId
                        && (p.AuditStatus == AuditStatusEnum.Active
                            || p.AuditStatus == AuditStatusEnum.Temporary))
                        .SingleOrDefaultAsync();

                    if (account == null)
                    {
                        throw new ApplicationException($"Account Id {command.AccountId} không tồn tại.");
                    }

                    account.AuditStatus = AuditStatusEnum.Deleted;

                    account.UpdatedDate = DateTime.UtcNow;

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
