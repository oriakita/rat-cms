namespace Raci.Web.BlazorServer.Shared.Directory
{
    using Blazored.Toast.Services;
    using Microsoft.AspNetCore.Components;
    using Raci.Common.Models;
    using System.Collections.Generic;
    using System.Linq;

    public static class AuthorizationHelper
    {
        public static UserPermissionsDto GetDefaultPagePermissions(List<RolePermissionDto> userPermissions,
            string uriAbsolutePath)
        {
            var authorization = new UserPermissionsDto();

            authorization.CanView = userPermissions
             .Any(p => HasPermissionTo(p, BitwiseActionId.View, uriAbsolutePath));

            authorization.CanAdd = userPermissions
              .Any(p => HasPermissionTo(p, BitwiseActionId.Add, uriAbsolutePath));

            authorization.CanEdit = userPermissions
              .Any(p => HasPermissionTo(p, BitwiseActionId.Edit, uriAbsolutePath));

            authorization.CanDelete = userPermissions
              .Any(p => HasPermissionTo(p, BitwiseActionId.Delete, uriAbsolutePath));

            authorization.CanImport = userPermissions
              .Any(p => HasPermissionTo(p, BitwiseActionId.Import, uriAbsolutePath));

            authorization.CanExport = userPermissions
              .Any(p => HasPermissionTo(p, BitwiseActionId.Export, uriAbsolutePath));

            return authorization;
        }

        public static bool GetCustomPermission(List<RolePermissionDto> userPermissions, int bitwiseActionId, string uriAbsolutePath)
        {
            return userPermissions.Any(p => HasPermissionTo(p, bitwiseActionId, uriAbsolutePath));
        }

        private static bool HasPermissionTo(RolePermissionDto permission, int actionId, string uriAbsolutePath)
        {
            var hasPermission = uriAbsolutePath.ToLower().StartsWith(permission.Function.Url)
                                      && (permission.ActionId & actionId) > 0;

            return hasPermission;

        }

        public static void RedirectToHomePageIfNotHavePermission(NavigationManager navigationManager, IToastService toastService)
        {
            navigationManager.NavigateTo("/");

            toastService.ShowWarning("You don't have permission to view this page!");
        }
    }

    public class UserPermissionsDto
    {
        public bool CanView { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanImport { get; set; }

        public bool CanExport { get; set; }
    }
}
