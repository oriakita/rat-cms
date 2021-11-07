using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using MediatR;
using Microsoft.AspNetCore.Components;
using Raci.Persistence;
using Raci.Web.BlazorServer.Shared.Directory;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Shared.Base
{
    public class DetailPageBase : ComponentBase
    {
        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [AllowNull]
        [Inject]
        protected IMediator _mediator { get; set; } = default;

        [Inject]
        protected SweetAlertService _sweetAlert { get; set; }

        [AllowNull]
        [Inject]
        protected IToastService _toastService { get; set; }

        [Inject]
        protected LoggingUser _loggingUser { get; set; }

        protected UserPermissionsDto UserPagePermissions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetPagePermissionsAsync();

                RedirectToHomePageIfNotHaveViewPermission();

                await base.OnInitializedAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void RedirectToHomePageIfNotHaveViewPermission()
        {
            if (!UserPagePermissions.CanView)
            {
                AuthorizationHelper.RedirectToHomePageIfNotHavePermission(_navigationManager, _toastService);
            }
        }

        //public void RedirectToHomePageIfNotHavePermission()
        //{
        //    _navigationManager.NavigateTo("/");

        //    _toastService.ShowWarning("You don't have permission to view this page!");
        //}

        protected virtual Task GetPagePermissionsAsync()
        {
            var uriAbsolutePath = new Uri(_navigationManager.Uri).AbsolutePath;
            UserPagePermissions = AuthorizationHelper.GetDefaultPagePermissions(_loggingUser.UserPermissions, uriAbsolutePath);

            return Task.CompletedTask;
        }
    }
}
