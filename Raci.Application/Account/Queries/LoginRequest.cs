using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Raci.Persistence;
using Raci.Service.JwtBlazor;

namespace Raci.Application.Account.Queries
{
    public class LoginRequest : IRequest<LoginRequest.ResponseDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public class Validator : AbstractValidator<LoginRequest>
        {
            public Validator()
            {

            }
        }

        public class Handler : IRequestHandler<LoginRequest, ResponseDto>
        {
            private readonly RaciDbContext _context;
            private readonly IAuthenticateService _authenticateService;
            public Handler(RaciDbContext context,
                IAuthenticateService authenticateService)
            {
                _context = context;
                _authenticateService = authenticateService;
            }

            public async Task<ResponseDto> Handle(LoginRequest request, CancellationToken cancellationToken)
            {
                var account = _context.RaciAccounts.SingleOrDefault(p => p.UserName == request.UserName);

                var loginResponse = new ResponseDto();

                if (account == null)
                {
                    loginResponse.Message = "Tài khoản không tồn tại";
                    return loginResponse;
                }

                var userManager = new UserManager(_context);

                if (await userManager.FindAsync(request.UserName, request.Password) == null)
                {
                    loginResponse.Message = "Sai tên tài khoản hoặc mật khẩu";
                    return loginResponse;
                }

                loginResponse.IsSuccessful = true;
                loginResponse.AccessToken = _authenticateService.GenerateAccessToken(account.Id, account.UserName, account.Email);

                return loginResponse;
            }
        }

        public class ResponseDto
        {
            public bool IsSuccessful { get; set; } = false;

            public string AccessToken { get; set; } = string.Empty;

            public string Message { get; set; } = string.Empty;
        }
    }
}
