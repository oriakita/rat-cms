using Microsoft.AspNetCore.Http;
using Raci.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserGuidId = Guid.Parse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            IsAuthenticated = UserGuidId != Guid.Empty;
        }

        public CurrentUserService(string userName, Guid userGuid)
        {
            UserName = userName;
            UserGuidId = userGuid;
        }

        public string UserName { get; set; }

        public Guid UserGuidId { get; set; }

        public bool IsAuthenticated { get; }
    }
}
