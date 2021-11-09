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
    public class UpdateAccountCommand : IRequest
    {
        public AccountDetailsGetByIdQuery.ResponseDto Account { get; set; }


        public class Validator : AbstractValidator<UpdateAccountCommand>
        {
            public Validator()
            {
                RuleFor(p => p.Account.UserName).NotNull().NotEmpty();
                RuleFor(p => p.Account.Password).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<UpdateAccountCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var account = await _context.RaciAccounts
                        .Where(p => p.Id == command.Account.AccountId
                        && (p.AuditStatus == AuditStatusEnum.Active
                            || p.AuditStatus == AuditStatusEnum.Temporary))
                        .SingleOrDefaultAsync();

                    if (account == null)
                    {
                        throw new ApplicationException($"Account Id {command.Account.AccountId} không tồn tại.");
                    }

                    var userManager = new UserManager(_context);

                    account.UserName = command.Account.UserName;
                    account.FirstName = command.Account.FirstName;
                    account.LastName = command.Account.LastName;
                    account.PhoneNumber = command.Account.PhoneNumber;
                    account.Email = command.Account.Email;
                    account.Gender = command.Account.Gender;
                    account.Role = command.Account.Role;
                    account.ShopGuid = command.Account.ShopGuid;
                    account.CreatedDate = DateTime.UtcNow;
                    account.UpdatedDate = DateTime.UtcNow;
                    account.AuditStatus = AuditStatusEnum.Active;
                    account.PasswordHash = userManager.GeneratePasswordHash(command.Account.Password, account.Salt);

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
