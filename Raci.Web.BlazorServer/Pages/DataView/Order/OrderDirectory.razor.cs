using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.DataView.Order
{
    public partial class OrderDirectory
    {
        [Inject]
        private RaciDbContext _context { get; set; }

        IEnumerable<OrderDto> orders;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                orders = await FetchDataDirectory();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<List<OrderDto>> FetchDataDirectory()
        {
            try
            {
                var orders = await _context.Orders
                        .Include(od => od.Shop)
                        .Include(od => od.Account)
                        .Select(p => new OrderDto
                        {
                            Id = p.Id,
                            CashAdvance = p.CashAdvance,
                            OrderStatus = p.OrderStatus,
                            TotalPay = p.TotalPay,
                            CustomerName = p.Account.FullName,
                            ShopName = p.Shop.Name,
                            CreatedDate = p.CreatedDate
                        }).ToListAsync();

                return orders;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
