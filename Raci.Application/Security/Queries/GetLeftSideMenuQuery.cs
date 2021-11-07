using MediatR;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Models;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raci.Application.Security.Queries
{
    public class GetLeftSideMenuQuery : IRequest<List<GetLeftSideMenuQuery.ResponseDto>>
    {
        public Guid AccountId { get; set; }

        public class Handler : IRequestHandler<GetLeftSideMenuQuery, List<ResponseDto>>
        {
            private readonly RaciDbContext _context;

            public Handler(RaciDbContext context)
            {
                _context = context;
            }

            public async Task<List<ResponseDto>> Handle(GetLeftSideMenuQuery request,
                CancellationToken cancellationToken)
            {
                try
                {
                    var userId = await _context.RaciAccounts
                        .Where(p => p.Id == request.AccountId)
                        .Select(p => p.Id)
                        .SingleOrDefaultAsync();

                    var query = _context.RolePermissions
                        .Where(p => p.Role.UserAssignments.Any(a => a.RaciAccountId == userId))
                        .Select(p => new
                        {
                            p.ActionId,
                            p.FunctionId,
                        });

                    var result = await query.ToListAsync();

                    var canSeePermissions = result
                        .Where(p => (p.ActionId & BitwiseActionId.View) > 0)
                        .ToList();

                    var canSeeFunctions = canSeePermissions.Select(p => p.FunctionId).ToList();

                    var canSeeModules = _context.Functions.Where(p => canSeeFunctions.Contains(p.Id)).Select(p => p.ModuleId).ToList();

                    var responseDtos = _context.Modules.Include(p => p.Functions)
                        .Where(p => canSeeModules.Contains(p.Id))
                        .Select(p => new ResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            DisplayName = p.DisplayName,
                            OrderNumber = p.OrderNumber,

                            RootPath = p.RootPath,

                            Icon = "",

                            Functions = p.Functions
                                .Where(function => !string.IsNullOrWhiteSpace(function.Url)
                                && canSeeFunctions.Contains(function.Id))
                                .OrderBy(f => f.OrderNumber)
                                .Select(function => new ResponseDto.FunctionDto
                                {
                                    Code = function.Code,
                                    Name = function.Name,

                                    AbsolutePath = function.Url,

                                })
                                .ToList()
                        })
                        .OrderBy(s => s.OrderNumber)
                        .ToList();

                    //foreach (var responseDto in responseDtos)
                    //{
                    //    var newRootPath = _commonSettings.PermissionRootPaths.SingleOrDefault(p => p.Source == responseDto.RootPath);

                    //    if (newRootPath == null) continue;

                    //    responseDto.RootPath = newRootPath.Destination;
                    //}

                    return responseDtos;
                }
                catch (Exception e)
                {

                    throw;
                }
            }

        }


        public class ResponseDto
        {
            public Guid Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            public string DisplayName { get; set; } = string.Empty;

            public int OrderNumber { get; set; }

            public string RootPath { get; set; }

            public string Icon { get; set; } = string.Empty;

            public List<FunctionDto> Functions { get; set; } = new List<FunctionDto>();

            public class FunctionDto
            {
                public string Code { get; set; } = string.Empty;

                public string Name { get; set; } = string.Empty;

                public string AbsolutePath { get; set; } = string.Empty;

                public string UrlWithouthFirstSlash
                {
                    get
                    {
                        if (string.IsNullOrWhiteSpace(this.AbsolutePath))
                        {
                            return this.AbsolutePath;
                        }

                        return this.AbsolutePath.Substring(1, this.AbsolutePath.Length - 1);
                    }
                }
            }
        }
    }
}
