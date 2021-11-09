using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.RaciSystem.RaciAccount
{
    public partial class RaciAccountDirectory
    {
        [Inject]
        private RaciDbContext _context { get; set; }

        private IEnumerable<RaciAccountDirectoryDto> _accounts;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                _accounts = await FetchDataDirectory();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<List<RaciAccountDirectoryDto>> FetchDataDirectory()
        {
            var items = await _context.RaciAccounts
                .Where(p => p.AuditStatus == AuditStatusEnum.Active || p.AuditStatus == AuditStatusEnum.Inactive)
                .Select(p => new RaciAccountDirectoryDto
                {
                    Id = p.Id,
                    Username = p.UserName,
                    Email = p.Email,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    Role = p.Role,
                    AuditStatus = p.AuditStatus,
                    CreatedDate = p.CreatedDate
                }).ToListAsync();

            return items;
        }
    }
}
