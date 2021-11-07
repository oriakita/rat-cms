using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Raci.Application.Item.Commands;
using Raci.Application.Item.Queries;
using Raci.Domain.Enums;
using Raci.Web.BlazorServer.Shared.Directory;
using Radzen;
using System;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Item
{
    public partial class ItemDetails
    {
        [Parameter]
        public string ItemId { get; set; }

        private ItemDetailsState _state = new ItemDetailsState();

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

                if (this.ItemId == null && !this.UserPagePermissions.CanAdd)
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
                var details = new ItemDetailsGetByIdQuery.ItemDetailDto();

                if (ItemId != null)
                {
                    details = await _mediator.Send(new ItemDetailsGetByIdQuery { ItemId = Guid.Parse(ItemId) });
                }
                else
                {
                    details.Id = Guid.NewGuid();
                }

                return details;

            });

            StateHasChanged();
        }

        private void SetItemSize(ItemSizeEnum value)
        {
            _state.Details.Size = value;
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

            if (string.IsNullOrWhiteSpace(_state.Details.NameVN))
            {
                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync("Không thể lưu, thiếu tên sản phẩm tiếng Việt!", icon: SweetAlertIcon.Error);
                return;
            }

            if (_state.Details.PriceVND == 0)
            {
                await _sweetAlert.HideLoadingAsync();
                await _sweetAlert.FireAsync("Không thể lưu, thiếu giá tiền VND của sản phẩm!", icon: SweetAlertIcon.Error);
                return;
            }

            try
            {
                if (ItemId == null)
                {
                    await _mediator.Send(new AddItemCommand
                    {
                        Item = _state.Details,
                        AccountId = _loggingUser.UserId
                    });

                    await _sweetAlert.HideLoadingAsync();

                    await _sweetAlert.FireAsync("Thêm thành công", icon: SweetAlertIcon.Success);

                    _navigationManager.NavigateTo($"/data-view/item/{_state.Details.Id.ToString()}", true);
                }
                else
                {
                    await _mediator.Send(new UpdateItemCommand
                    {
                        Item = _state.Details,
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
                Text = $"Có phải bạn muốn xóa món {_state.Details.NameVN}",
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

                await _mediator.Send(new DeleteItemCommand
                {
                    ItemId = _state.Details.Id,
                    AccountId = _loggingUser.UserId
                });

                await _sweetAlert.HideLoadingAsync();

                await _sweetAlert.FireAsync("Xóa thành công", icon: SweetAlertIcon.Success);

                _navigationManager.NavigateTo($"/data-view/item", true);

            }
            catch (Exception ex)
            {
                _toastService.ShowError(ex.Message);
            }
        }
    }
}
