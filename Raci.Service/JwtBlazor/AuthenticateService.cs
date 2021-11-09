namespace Raci.Service.JwtBlazor
{
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class AuthenticateService : IAuthenticateService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHanler = new JwtSecurityTokenHandler();
        private readonly TokenValidationParameters _tokenValidationParameters = new TokenValidationParameters();
        private readonly JwtSetting _tokenManagement;

        public AuthenticateService(JwtSetting tokenManagement)
        {
            _tokenManagement = tokenManagement;

            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenManagement.SecretKey)),
                ValidateIssuer = true,
                ValidateAudience = true,

                ValidAudience = _tokenManagement.Audience,
                ValidIssuer = _tokenManagement.Issuer,
            };
        }

        public string GenerateAccessToken(Guid userGuid, string userName, string email)
        {
            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userGuid.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email ?? ""),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;

        }

        public ClaimsPrincipal Validate(string accessToken)
        {
            try
            {
                var claimsPrincipal = _jwtSecurityTokenHanler.ValidateToken(
                    accessToken,
                    this._tokenValidationParameters,
                    out var securityToken);

                return claimsPrincipal;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
