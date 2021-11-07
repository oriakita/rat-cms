namespace Raci.Service.JwtBlazor
{
    using System;
    using System.Security.Claims;
    public interface IAuthenticateService
    {
        string GenerateAccessToken(Guid userGuid, string userName, string email);

        ClaimsPrincipal Validate(string accessToken);
    }
}
