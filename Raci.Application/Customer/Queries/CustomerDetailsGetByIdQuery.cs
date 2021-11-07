using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Domain.Enums;
using Raci.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Customer.Queries
{
    public class CustomerDetailsGetByIdQuery : IRequest<CustomerDetailsGetByIdQuery.CustomerDetailDto>
    {
        public Guid CustomerId { get; set; }

        public class Handler : IRequestHandler<CustomerDetailsGetByIdQuery, CustomerDetailDto>
        {
            private readonly RaciDbContext _context;
            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<CustomerDetailDto> Handle(CustomerDetailsGetByIdQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var account = await _context.Accounts
                        .Where(p => p.Id == query.CustomerId
                        && p.AuditStatus != AuditStatusEnum.Deleted
                        && p.AuditStatus != AuditStatusEnum.Undefined)
                        .SingleOrDefaultAsync();

                    if (account == null)
                    {
                        throw new ApplicationException($"Khách hàng Id {query.CustomerId} không tồn tại.");
                    }

                    var response = new CustomerDetailDto
                    {
                        Id = account.Id,
                        PhoneNumber = account.PhoneNumber,
                        CountryCallingCode = account.CountryCallingCode,
                        UserName = account.UserName,
                        Avatar = account.Avatar,
                        Email = account.Email,
                        FullName = account.FullName,
                        Gender = account.Gender,
                        LanguageCode = account.LanguageCode,
                        UserType = account.UserType,
                        AuditStatus = account.AuditStatus
                    };

                    return response;
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Lỗi khi truy cập thông tin");
                }
            }
        }

        public class CustomerDetailDto
        {
            public Guid Id { get; set; }

            public string PhoneNumber { get; set; }

            public string UserName { get; set; }

            public string CountryCallingCode { get; set; }

            public GenderEnum Gender { get; set; }

            public UserTypeEnum UserType { get; set; }

            public string Avatar { get; set; }

            public string Email { get; set; }

            public string FullName { get; set; }
                
            public string LanguageCode { get; set; }

            public AuditStatusEnum AuditStatus { get; set; }
        }
    }
}
