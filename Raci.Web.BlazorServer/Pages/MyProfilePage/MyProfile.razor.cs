using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Raci.Application.Account.Commands;
using Raci.Application.Account.Queries;
using Raci.Persistence;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.MyProfilePage
{
    public partial class MyProfile
    {
        [Inject]
        protected LoggingUser LoggingUser { get; set; }

        private MyProfileState _state = new MyProfileState();

        private string _oldPassword = string.Empty;
        private string _newPassword = string.Empty;
        private string _repeatPassword = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {
                    await FetchDataAsync();
                }
            }
            catch (Exception ex)
            {
                _toastService.ShowError(ex.Message);
            }
        }

        private async Task FetchDataAsync()
        {
            await _state.SetStateAsync(async () =>
            {
                var details = await _mediator.Send(new GetMyProfileQuery
                { 
                    AccountGuid = LoggingUser.UserId
                });

                return details;

            });

            StateHasChanged();
        }

        private async Task OnProgress(UploadProgressArgs args, string name)
        {
            if (args.Progress == 100)
            {
                await Task.Delay(2000);

                await FetchDataAsync();
            }
        }

        private async Task HandleOnUpdateProfileButtonClicked()
        {
            await _sweetAlert.ShowLoadingAsync();

            try
            {
                await _mediator.Send(new UpdateProfileCommand
                {
                    Profile = _state.Details
                });

                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync("Cập nhật thành công", icon: SweetAlertIcon.Success);
            }
            catch (Exception ex)
            {
                await _sweetAlert.HideLoadingAsync();
                _toastService.ShowError(ex.Message);
            }
        }

        private async Task HandleOnUpdatePasswordButtonClicked()
        {
            await _sweetAlert.ShowLoadingAsync();

            if (string.IsNullOrWhiteSpace(_oldPassword)
                || string.IsNullOrWhiteSpace(_newPassword)
                || string.IsNullOrWhiteSpace(_repeatPassword))
            {
                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync("Mật khẩu không được để trống.", icon: SweetAlertIcon.Error);
                return;
            }

            if (_newPassword != _repeatPassword)
            {
                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync("Mật khẩu nhập lại không giống nhau.", icon: SweetAlertIcon.Error);
                return;
            }

            try
            {
                await _mediator.Send(new UpdateAccountPasswordCommand
                {
                    AccountId = _state.Details.AccountId,
                    OldPassword = _oldPassword,
                    NewPassword = _newPassword
                });

                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync("Cập nhật thành công", icon: SweetAlertIcon.Success);
            }
            catch (Exception ex)
            {
                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync(ex.Message, icon: SweetAlertIcon.Error);
            }
        }
    }
}
