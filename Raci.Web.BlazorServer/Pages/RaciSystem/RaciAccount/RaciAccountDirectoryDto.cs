
using Raci.Common.Enums;
using Raci.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.RaciSystem.RaciAccount
{
    public class RaciAccountDirectoryDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public GenderEnum Gender { get; set; }

        public RoleEnum Role { get; set; }

        public AuditStatusEnum AuditStatus { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
