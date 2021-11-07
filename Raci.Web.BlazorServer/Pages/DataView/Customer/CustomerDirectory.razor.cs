using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Customer
{
    public partial class CustomerDirectory
    {

        [Inject]
        private RaciDbContext _context { get; set; }

        IEnumerable<CustomerDto> customers;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                customers = await FetchDataDirectory();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<List<CustomerDto>> FetchDataDirectory()
        {
            var customers = await _context.Accounts
                .Where(p => p.AuditStatus != AuditStatusEnum.Deleted && p.AuditStatus != AuditStatusEnum.Undefined)
                .Select(p => new CustomerDto
                {
                    Id = p.Id,
                    PhoneNumber = p.PhoneNumber,
                    CountryCallingCode = p.CountryCallingCode,
                    UserName = p.UserName,
                    FullName = p.FullName,
                    Email = p.Email,
                    AuditStatus = p.AuditStatus
                }).ToListAsync();

            return customers;
        }
    }
}
