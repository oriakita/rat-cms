using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Raci.Domain.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Shared.Components
{
    public partial class RoleSelector
    {
        [Inject]
        private RaciDbContext _context { get; set; }

        [Parameter]
        public EventCallback<Guid> ValueChanged { get; set; }

        [Parameter]
        public Guid? Value { get; set; }

        private List<RoleSelectorDto> _allRoles = new List<RoleSelectorDto>();

        private RoleSelectorDto _selectedRole = new RoleSelectorDto();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _allRoles = await _context.Roles
                    .Where(p => p.Status == StatusEnum.Active || p.Status == StatusEnum.Locked)
                    .Select(p => new RoleSelectorDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected override void OnParametersSet()
        {
            _selectedRole = _allRoles.SingleOrDefault(p => p.Id == Value);
        }

        private async Task OnChange(object value)
        {
            _selectedRole = _allRoles.SingleOrDefault(p => p.Id == Guid.Parse(value.ToString()));

            await ValueChanged.InvokeAsync(_selectedRole.Id);
        }

        public class RoleSelectorDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}
