using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Application.Models;
using Raci.Common.Models;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Security.Queries
{
    public class GetRoleBasedPermissionByAccountIdQuery : IRequest<List<RolePermissionDto>>
    {
        public Guid AccountId { get; set; }

        public class Handler : IRequestHandler<GetRoleBasedPermissionByAccountIdQuery, List<RolePermissionDto>>
        {
            private readonly RaciDbContext _context;
            private readonly IMapper _mapper;

            public Handler(RaciDbContext context, IMapper mapper)
            {
                _context = context;
                this._mapper = mapper;
            }

            public async Task<List<RolePermissionDto>> Handle(GetRoleBasedPermissionByAccountIdQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var rolePermissionModels = new List<RolePermissionDto>();

                    var rolePermissions = await _context.RolePermissions
                        .Where(p => p.Role.UserAssignments.Any(a => a.RaciAccount.Id == query.AccountId))
                        .Include(s => s.Function)
                            .ThenInclude(s => s.Module)
                                .ThenInclude(s => s.Functions).ThenInclude(s => s.Actions)
                        .ToListAsync();

                    rolePermissionModels = _mapper.Map<List<RolePermissionDto>>(rolePermissions);

                    foreach (var rolePermission in rolePermissionModels)
                    {
                        foreach (var action in rolePermission.Function.Actions)
                        {
                            action.HasPermission = (rolePermission.ActionId & action.ActionId) > 0;
                        }
                    }

                    return rolePermissionModels.ToList();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
    }
}
