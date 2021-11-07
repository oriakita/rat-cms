using AutoMapper;
using Raci.Common.Models;
using Raci.Domain.RaciAccountAggregate;
using Raci.Domain.SecurityAggregate;

namespace Raci.Application.Models.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Module, ModuleModel>();
            CreateMap<ModuleModel, Module>();

            CreateMap<Action, ActionModel>();
            CreateMap<ActionModel, Action>();

            CreateMap<RaciAccount, RaciAccountModel>();

            CreateMap<Function, FunctionModel>();
            CreateMap<FunctionModel, Function>();

            CreateMap<RolePermission, RolePermissionDto>();
            CreateMap<RolePermissionDto, RolePermission>();

            CreateMap<UserAssignment, UserAssignmentModel>();
        }
    }
}
