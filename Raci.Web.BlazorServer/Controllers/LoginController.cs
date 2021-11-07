using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Raci.Service.JwtBlazor;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Raci.Web.BlazorServer.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthenticateService _authenticateService;
        
        public LoginController(AuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpGet("/login/sign-in")]
        public async Task<ActionResult> LoginFromRaciCookieAsync([FromQuery]string accessToken)
        {
            //var paramters = HttpUtility.UrlEncode($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/login/login-from-raci-cookie?redirectUrl={redirectUrl}");
            //var encodedRedirectUrl = _hipsSettings.HipsV1RootPathWithouthLastSlash + $"/LoginV2?hipsV2RedirectUrl=" + paramters;
            // encodedRedirectUrl = HttpUtility.UrlEncode(encodedRedirectUrl);

            try
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return Redirect("/");
                }

                var jwtToken = _authenticateService.Validate(accessToken);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                foreach (var claim in jwtToken.Claims)
                {
                    identity.AddClaim(claim);
                }

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Redirect("/home");
            }
            catch (Exception e)
            {
                return Redirect("/");
            }

        }

        [HttpGet("/login/is-signed-in")]
        public async Task<bool> IsSignedIn()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [HttpGet("log-out")]
        public async Task<ActionResult> LogOutFromRaciCookieAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("RACIForm");

            return Redirect("/");
        }

        //public ActionResult Index()
        //{
        //    return Challenge(new AuthenticationProperties() { RedirectUri = "/" },
        //      Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme);
        //}

        //public ActionResult Logout()
        //{
        //    return SignOut(new AuthenticationProperties() { RedirectUri = "/" },
        //        Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme,
        //        Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        //}
    }
}
