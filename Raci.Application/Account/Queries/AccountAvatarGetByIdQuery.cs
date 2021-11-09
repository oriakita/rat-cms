using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Domain.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Account.Queries
{
    public class AccountAvatarGetByIdQuery : IRequest<string>
    {
        public Guid AccountGuid { get; set; }

        public class Handler : IRequestHandler<AccountAvatarGetByIdQuery, string>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(AccountAvatarGetByIdQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var account = await _context.RaciAccounts
                        .AsNoTracking()
                        .Where(p => p.Id == query.AccountGuid
                                && p.AuditStatus == AuditStatusEnum.Active)
                        .SingleOrDefaultAsync();

                    if (account == null) return "";

                    return account.Avatar;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập avatar");
                }
            }
        }
    }
}
