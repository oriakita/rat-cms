using Raci.Common.Enums;
using Raci.Domain.Enums;
using Raci.Domain.OrderAggregate;
using Raci.Domain.SecurityAggregate;
using Raci.Domain.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Domain.RaciAccountAggregate
{
    public class RaciAccount
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public GenderEnum Gender { get; set; }

        public string Avatar { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; } = "mysalt2020abcdef";

        public string LanguageCode { get; set; }

        public RoleEnum Role { get; set; }

        public string Office { get; set; }

        public string Department { get; set; }

        public AuditStatusEnum AuditStatus { get; set; }

        public DateTime? LastAccessDate { get; set; }

        //public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public bool PhoneNumberConfirmed { get; set; }

        //public int? OfficeId { get; set; }

        public DateTime? UpdatedPasswordDate { get; set; }

        //public string Otp { get; set; }

        //public DateTime? CreatedDateOtp { get; set; }

        public Guid? ShopGuid { get; set; }

        public Shop? Shop { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public ICollection<UserAssignment> UserAssignments { get; set; }
    }
}
