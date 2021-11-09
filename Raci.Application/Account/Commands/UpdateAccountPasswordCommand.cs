using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Account.Commands
{
    public class UpdateAccountPasswordCommand : IRequest
    {
        public Guid AccountId { get; set; }

        public string OldPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        public class Validator : AbstractValidator<UpdateAccountPasswordCommand>
        {
            public Validator()
            {
                RuleFor(p => p.OldPassword).NotNull().NotEmpty();
                RuleFor(p => p.NewPassword).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<UpdateAccountPasswordCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateAccountPasswordCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var account = await _context.RaciAccounts
                        .Where(p => p.Id == command.AccountId
                            && p.AuditStatus == AuditStatusEnum.Active)
                        .SingleOrDefaultAsync();

                    if (account == null)
                    {
                        throw new ApplicationException($"Tài khoản Id {command.AccountId} không tồn tại.");
                    }

                    var userManager = new UserManager(_context);

                    if (!userManager.IsMatchPassword(account.PasswordHash, account.Salt, command.OldPassword))
                    {
                        throw new ApplicationException($"Mật khẩu cũ không đúng");
                    }

                    account.PasswordHash = userManager.GeneratePasswordHash(command.NewPassword, account.Salt);

                    account.UpdatedPasswordDate = DateTime.UtcNow;
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
