using Microsoft.AspNetCore.Components;
using Raci.Application.Customer.Queries;
using Raci.Web.BlazorServer.Shared.Directory;
using System;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Customer
{
    public partial class CustomerDetails
    {
        [Parameter]
        public string CustomerId { get; set; }

        private CustomerDetailsState _state = new CustomerDetailsState();

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

                if (this.CustomerId == null && !this.UserPagePermissions.CanAdd)
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
                var details = new CustomerDetailsGetByIdQuery.CustomerDetailDto();

                if (CustomerId != null)
                {
                    details = await _mediator.Send(new CustomerDetailsGetByIdQuery { CustomerId = Guid.Parse(CustomerId) });
                }

                return details;

            });

            StateHasChanged();
        }
    }
}
