using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Raci.Application.Shop.Queries;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Raci.Application.Shop.Queries.GetShopSelectorQuery;

namespace Raci.Web.BlazorServer.Shared.Components
{
    public partial class ShopSelector
    {

        [Inject]
        private RaciDbContext _context { get; set; }

        [Inject]
        private IMediator _mediator { get; set; }


        [Parameter]
        public EventCallback<Guid?> ValueChanged { get; set; }

        [Parameter]
        public Guid? Value { get; set; }

        private List<ShopTinyDto> _shop = new List<ShopTinyDto>();

        private ShopTinyDto _selectedShop = new ShopTinyDto();

        //protected override async Task OnInitializedAsync()
        //{
        //    try
        //    {
                
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {
                    _shop = await GetShopDataSourceAsync();
                    var selectedShop = _shop.SingleOrDefault(p => p.Id == Value);
                    if (selectedShop != null) _selectedShop = selectedShop;

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task<List<ShopTinyDto>> GetShopDataSourceAsync()
        {
            var shops = await _mediator.Send(new GetShopSelectorQuery());

            return shops;
        }

        //protected override void OnParametersSet()
        //{
        //    _selectedShop = _shop.SingleOrDefault(p => p.Id == Value);
        //}

        private async Task OnChange(object value)
        {
            try
            {
                if (value == null || Guid.Parse(value.ToString()) == Guid.Empty)
                {
                    _selectedShop = new ShopTinyDto();
                    await ValueChanged.InvokeAsync(null);
                    return;
                }

                var shop = _shop.SingleOrDefault(p => p.Id == Guid.Parse(value.ToString()));

                if (shop != null) _selectedShop = shop;

                await ValueChanged.InvokeAsync(_selectedShop.Id);
            }
            catch (Exception e)
            {

                throw;
            }
        }

    }
}
