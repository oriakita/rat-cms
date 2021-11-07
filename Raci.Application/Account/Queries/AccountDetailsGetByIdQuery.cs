using MediatR;
using System;
using System.Collections.Generic;
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
            public Handler()
            {

            }

            public async Task<ResponseDto> Handle(AccountDetailsGetByIdQuery query, CancellationToken cancellationToken)
            {
                return null;
            }
        }

        public class ResponseDto
        {

        }
    }
}
