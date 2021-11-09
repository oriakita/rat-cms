using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Raci.Application.Account.Commands;
using Raci.Application.Account.Queries;
using Radzen;
using System;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.RaciSystem.RaciAccount
{
    public partial class RaciAccountDetails
    {
        [Parameter]
        public string AccountId { get; set; }

        private RaciAccountDetailsState _state = new RaciAccountDetailsState();

        private Guid _newAccountId = Guid.NewGuid();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {
                    await FetchDataAsync();

                    StateHasChanged();
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
                var details = new AccountDetailsGetByIdQuery.ResponseDto();
                if (AccountId != null)
                {
                    details = await _mediator.Send(new AccountDetailsGetByIdQuery
                    {
                        AccountGuid = Guid.Parse(AccountId)
                    });
                }
                else
                {
                    try
                    {
                        details = await _mediator.Send(new AccountDetailsGetByIdQuery
                        {
                            AccountGuid = _newAccountId
                        });
                    }
                    catch (Exception)
                    {
                        details.AccountId = _newAccountId;
                    }
                }

                return details;
            });
        }

        private async Task OnProgress(UploadProgressArgs args, string name)
        {
            if (args.Progress == 100)
            {
                await Task.Delay(2000);

                await FetchDataAsync();
            }
        }

        private async Task HandleOnAddOrUpdateButtonClicked()
        {
            await _sweetAlert.ShowLoadingAsync();

            if (string.IsNullOrWhiteSpace(_state.Details.UserName))
            {
                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync("Không thể lưu, thiếu Username", icon: SweetAlertIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(_state.Details.Password))
            {
                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync("Không thể lưu, thiếu mật khẩu", icon: SweetAlertIcon.Error);
                return;
            }

            try
            {
                if (AccountId == null)
                {
                    await _mediator.Send(new AddNewAccountCommand
                    {
                        Account = _state.Details
                    });

                    await _sweetAlert.HideLoadingAsync();

                    await _sweetAlert.FireAsync("Thêm thành công", icon: SweetAlertIcon.Success);

                    _navigationManager.NavigateTo($"/raci-system/account/{_state.Details.AccountId.ToString()}", true);
                }
                else
                {
                    await _mediator.Send(new UpdateAccountCommand
                    {
                        Account = _state.Details,
                    });

                    StateHasChanged();

                    await _sweetAlert.HideLoadingAsync();

                    await _sweetAlert.FireAsync("Cập nhật thành công", icon: SweetAlertIcon.Success);
                }

            }
            catch (Exception ex)
            {
                _toastService.ShowError(ex.Message);
            }
        }

        private async Task HandleOnDeleteButtonClicked()
        {
            var sweetAlertResult = await _sweetAlert.FireAsync(new SweetAlertOptions
            {
                ShowConfirmButton = true,
                Text = $"Có phải bạn muốn xóa tài khoản {_state.Details.UserName}",
                ConfirmButtonText = "Có",
                CancelButtonText = "Hủy",
                ShowCancelButton = true
            });

            if (sweetAlertResult.Dismiss != null)
            {
                return;
            }

            await _sweetAlert.ShowLoadingAsync();

            try
            {

                await _mediator.Send(new DeleteAccountCommand
                {
                    AccountId = _state.Details.AccountId
                });

                await _sweetAlert.HideLoadingAsync();

                await _sweetAlert.FireAsync("Xóa thành công", icon: SweetAlertIcon.Success);

                _navigationManager.NavigateTo($"/raci-system/account", true);

            }
            catch (Exception ex)
            {
                _toastService.ShowError(ex.Message);
            }
        }
    }
}
