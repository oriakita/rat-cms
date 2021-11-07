using MediatR;
using Microsoft.AspNetCore.Components;
using Raci.Application.Order.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.Dashboard.OrderProcess
{
    public partial class OrderProcess
    {
        [Inject]
        private IMediator _mediator { get; set; }

        //[Inject]
        //private RaciDbContext _context { get; set; }

        private IEnumerable<OrderOnProcessingGetAllQuery.OrderByShopDto> _orderByShops = new List<OrderOnProcessingGetAllQuery.OrderByShopDto>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await FetchDataDirectory();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task FetchDataDirectory()
        {
            _orderByShops = await _mediator.Send(new OrderOnProcessingGetAllQuery());
        }
    }
}
