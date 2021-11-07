using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Raci.Application.Order.Commands;
using Raci.Application.Order.Queries;
using Raci.Web.BlazorServer.Shared.Directory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.Dashboard.OrderProcess
{
    public partial class OrderProcessDetail
    {
        [Parameter]
        public string OrderId { get; set; }

        private OrderProcessDetailState _state = new OrderProcessDetailState();

        private double _cashAdvance = 0;

        private double _cashChange = 0;

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
                var details = new OrderDetailsGetByIdQuery.OrderDetailDto();

                if (OrderId != null)
                {
                    details = await _mediator.Send(new OrderDetailsGetByIdQuery { OrderId = Guid.Parse(OrderId) });
                }

                return details;

            });

            StateHasChanged();
        }

        private async Task OnCashAdvanceChanged(ChangeEventArgs e)
        {
            _cashAdvance = double.Parse(e.Value.ToString());

            _cashChange = _cashAdvance - _state.Details.TotalPay;

            StateHasChanged();
        }

        private async Task HandleOnCancelButtonClicked()
        {
            var sweetAlertResult = await _sweetAlert.FireAsync(new SweetAlertOptions
            {
                ShowConfirmButton = true,
                Text = $"Bạn có muốn hủy bỏ đơn hàng không?",
                ConfirmButtonText = "Có",
                CancelButtonText = "Không",
                ShowCancelButton = true
            });

            if (sweetAlertResult.Dismiss != null)
            {
                return;
            }

            await _sweetAlert.ShowLoadingAsync();

            try
            {

                await _mediator.Send(new CancelOrderCommand
                {
                    OrderId = _state.Details.Id,
                    AccountId = _loggingUser.UserId
                });

                await _sweetAlert.HideLoadingAsync();

                await _sweetAlert.FireAsync("Đã hủy bỏ đơn hàng", icon: SweetAlertIcon.Info);

                _navigationManager.NavigateTo($"/order-process", true);

            }
            catch (Exception ex)
            {
                _toastService.ShowError(ex.Message);
            }
        }

        private async Task HandleOnPaidButtonClicked()
        {
            if (_cashAdvance < _state.Details.TotalPay)
            {
                await _sweetAlert.FireAsync(new SweetAlertOptions
                {
                    Html = "Khách không đưa đủ số tiền thanh toán! Không thể hoàn thành đơn hàng.",
                    Icon = SweetAlertIcon.Error,
                });

                return;
            }

            await _sweetAlert.ShowLoadingAsync();

            try
            {

                await _mediator.Send(new PaidOrderCommand
                {
                    OrderId = _state.Details.Id,
                    AccountId = _loggingUser.UserId,
                    CashAdvance = _cashAdvance,
                    Change = _cashChange
                });

                await _sweetAlert.HideLoadingAsync();

                await _sweetAlert.FireAsync("Đã hoàn thành đơn hàng", icon: SweetAlertIcon.Success);

                _navigationManager.NavigateTo($"/order-process", true);

            }
            catch (Exception ex)
            {
                _toastService.ShowError(ex.Message);
            }
        }
    }
}
