using Microsoft.AspNetCore.Components;
using Raci.Web.BlazorServer.Shared.Directory;
using System;
using System.Threading.Tasks;
using Raci.Application.Shop.Queries;
using CurrieTechnologies.Razor.SweetAlert2;
using Raci.Application.Shop.Commands;

namespace Raci.Web.BlazorServer.Pages.DataView.Shop
{
    public partial class ShopDetails
    {
        [Parameter]
        public string ShopId { get; set; }

        private ShopDetailsState _state = new ShopDetailsState();

        void OnChange(int index)
        {
            
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);

                if (this.ShopId == null && !this.UserPagePermissions.CanAdd)
                {
                    AuthorizationHelper.RedirectToHomePageIfNotHavePermission(_navigationManager, _toastService);
                }

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
                var details = new ShopDetailsGetByIdQuery.ShopDetailDto();

                if (ShopId != null)
                {
                    details = await _mediator.Send(new ShopDetailsGetByIdQuery { ShopId = Guid.Parse(ShopId) });
                }
                else
                {
                    details.Id = Guid.NewGuid();
                }

                return details;

            });

            StateHasChanged();
        }

        private async Task HandleOnAddOrUpdateButtonClicked()
        {
            await _sweetAlert.ShowLoadingAsync();

            if (string.IsNullOrWhiteSpace(_state.Details.Name) || string.IsNullOrWhiteSpace(_state.Details.Address))
            {
                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync("Không thể lưu, cần điền đủ thông tin", icon: SweetAlertIcon.Error);
                return;
            }

            try
            {
                if (ShopId == null)
                {
                    await _mediator.Send(new AddShopCommand
                    {
                        Shop = _state.Details,
                        AccountId = _loggingUser.UserId
                    });

                    await _sweetAlert.HideLoadingAsync();

                    await _sweetAlert.FireAsync("Thêm thành công", icon: SweetAlertIcon.Success);

                    _navigationManager.NavigateTo($"/data-view/shop/{_state.Details.Id.ToString()}", true);
                }
                else
                {
                    await _mediator.Send(new UpdateShopCommand
                    {
                        Shop = _state.Details,
                        AccountId = _loggingUser.UserId
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
                Text = $"Có phải bạn muốn xóa cửa hàng {_state.Details.Name}",
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

                await _mediator.Send(new DeleteShopCommand
                {
                    ShopId = _state.Details.Id,
                    AccountId = _loggingUser.UserId
                });

                await _sweetAlert.HideLoadingAsync();

                await _sweetAlert.FireAsync("Xóa thành công", icon: SweetAlertIcon.Success);

                _navigationManager.NavigateTo($"/data-view/shop", true);

            }
            catch (Exception ex)
            {
                _toastService.ShowError(ex.Message);
            }
        }
    }
}
