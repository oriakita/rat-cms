using Raci.Common.Models;
using System;
using System.Collections.Generic;

namespace Raci.Persistence
{
    public class LoggingUser
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public Guid UserId { get; set; }

        public string LanguageCode { get; set; } = "vi";

        public string Avatar { get; set; } = string.Empty;

        public List<RolePermissionDto> UserPermissions { get; set; } = new List<RolePermissionDto>();
    }
}
