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
    public class UpdateProfileCommand : IRequest
    {
        public AccountDetailsGetByIdQuery.ResponseDto Profile { get; set; }

        //public Guid AccountId { get; set; }

        public class Validator : AbstractValidator<UpdateProfileCommand>
        {
            public Validator()
            {
                RuleFor(p => p.Profile.FirstName).NotNull().NotEmpty();
                RuleFor(p => p.Profile.LastName).NotNull().NotEmpty();
                RuleFor(p => p.Profile.Email).NotNull().NotEmpty();
                RuleFor(p => p.Profile.PhoneNumber).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<UpdateProfileCommand>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var account = await _context.RaciAccounts
                        .Where(p => p.Id == command.Profile.AccountId
                            && p.AuditStatus == AuditStatusEnum.Active)
                        .SingleOrDefaultAsync();

                    if (account == null)
                    {
                        throw new ApplicationException($"Tài khoản Id {command.Profile.AccountId} không tồn tại.");
                    }

                    account.FirstName = command.Profile.FirstName;
                    account.LastName = command.Profile.LastName;
                    account.Email = command.Profile.Email;
                    account.PhoneNumber = command.Profile.PhoneNumber;
                    account.Gender = command.Profile.Gender;

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
