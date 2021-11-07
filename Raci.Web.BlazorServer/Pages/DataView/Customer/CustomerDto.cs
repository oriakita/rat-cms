using Raci.Common.Enums;
using System;

namespace Raci.Web.BlazorServer.Pages.DataView.Customer
{
    public class CustomerDto
    {
        public Guid Id { get; set; }

        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        public string CountryCallingCode { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public AuditStatusEnum AuditStatus { get; set; }
    }
}
