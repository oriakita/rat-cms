using Microsoft.AspNetCore.Components;
using Raci.Application.Order.Queries;
using Raci.Web.BlazorServer.Shared.Directory;
using System;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Order
{
    public partial class OrderDetails
    {
        [Parameter]
        public string OrderId { get; set; }

        private OrderDetailsState _state = new OrderDetailsState();

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

                if (this.OrderId == null)
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
                var details = new OrderDetailsGetByIdQuery.OrderDetailDto();

                if (OrderId != null)
                {
                    details = await _mediator.Send(new OrderDetailsGetByIdQuery { OrderId = Guid.Parse(OrderId) });
                }

                return details;

            });

            StateHasChanged();
        }

        //private string CalculateTotalPrice()
        //{
        //    double total = 0;

        //    if (this._state.Details.OrderItemDetails != null)
        //    {
        //        foreach (var detail in _state.Details.OrderItemDetails)
        //        {
        //            total = total + (detail.Quantity * detail.UnitPrice);
        //        }
        //    }

        //    return String.Format("{0:n0}", total);
        //}
    }
}
