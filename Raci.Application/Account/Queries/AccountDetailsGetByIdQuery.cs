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
    public class AccountDetailsGetByIdQuery : IRequest<AccountDetailsGetByIdQuery.ResponseDto>
    {
        public Guid AccountGuid { get; set; }

        public class Handler : IRequestHandler<AccountDetailsGetByIdQuery, ResponseDto>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<ResponseDto> Handle(AccountDetailsGetByIdQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var account = await _context.RaciAccounts
                        .Include(p => p.Shop)
                        .AsNoTracking()
                        .Where(p => p.Id == query.AccountGuid
                        && (p.AuditStatus == AuditStatusEnum.Active
                            || p.AuditStatus == AuditStatusEnum.Temporary))
                        .SingleOrDefaultAsync();

                    if (account == null)
                    {
                        throw new ApplicationException($"Account Id {query.AccountGuid} không tồn tại.");
                    }

                    var response = new ResponseDto
                    {
                        AccountId = account.Id,
                        UserName = account.UserName,
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        PhoneNumber = account.PhoneNumber,
                        Email = account.Email,
                        Gender = account.Gender,
                        Role = account.Role,
                        Avatar = account.Avatar,
                        ShopGuid = account.Shop?.Id,
                        ShopAddress = account.Shop?.Address,
                        Password = account.PasswordHash
                    };

                    return response;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập thông tin");
                }
            }
        }

        public class ResponseDto
        {
            public Guid AccountId { get; set; }
            public string UserName { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public GenderEnum Gender { get; set; } = GenderEnum.NotSet;
            public RoleEnum Role { get; set; } = RoleEnum.Sale;
            public string Avatar { get; set; } = string.Empty;
            public Guid? ShopGuid { get; set; }
            public string ShopAddress { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
