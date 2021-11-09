using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Application.Account.Queries;
using Raci.Common.Enums;
using Raci.Domain.RaciAccountAggregate;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Account.Commands
{
    public class AddNewAccountCommand : IRequest
    {
        //public Guid AccountId { get; set; }

        public AccountDetailsGetByIdQuery.ResponseDto Account { get; set; }

        public class Validator : AbstractValidator<AddNewAccountCommand>
        {
            public Validator()
            {
                RuleFor(p => p.Account.UserName).NotNull().NotEmpty();
                RuleFor(p => p.Account.Password).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<AddNewAccountCommand>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AddNewAccountCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var account = await _context.RaciAccounts
                        .Where(p => p.Id == command.Account.AccountId
                        && (p.AuditStatus == AuditStatusEnum.Active
                            || p.AuditStatus == AuditStatusEnum.Temporary))
                        .SingleOrDefaultAsync();

                    var userManager = new UserManager(_context);

                    if (account != null)
                    {
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
                    }
                    else
                    {
                        var newAccount = new RaciAccount
                        {
                            Id = command.Account.AccountId,
                            UserName = command.Account.UserName,
                            FirstName = command.Account.FirstName,
                            LastName = command.Account.LastName,
                            PhoneNumber = command.Account.PhoneNumber,
                            Email = command.Account.Email,
                            Gender = command.Account.Gender,
                            Role = command.Account.Role,
                            Avatar = command.Account.Avatar,
                            ShopGuid = command.Account.ShopGuid,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            AuditStatus = AuditStatusEnum.Active
                        };

                        newAccount.PasswordHash = userManager.GeneratePasswordHash(command.Account.Password, newAccount.Salt);

                        await _context.RaciAccounts.AddAsync(newAccount);
                    }

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
