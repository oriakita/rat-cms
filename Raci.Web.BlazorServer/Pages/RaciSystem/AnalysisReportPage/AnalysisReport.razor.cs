using Raci.Application.Order.Queries;
using Raci.Web.BlazorServer.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.RaciSystem.AnalysisReportPage
{
    public partial class AnalysisReport
    {
        private DateTime? _selectedDate = DateTime.Now;
        private DateTime? _selectedMonth = DateTime.Now;

        private Guid? _selectedShopIdDateSection;
        private Guid? _selectedShopIdMonthSection;

        private ReportSectionState<GetOrderReportByShopAndByDateQuery.OrderReportDto> _orderReportByDateAndShopState 
            = new ReportSectionState<GetOrderReportByShopAndByDateQuery.OrderReportDto>();

        private ReportSectionState<List<GetOrderReportByShopAndByMonthQuery.DataOrderReportInDayDto>> _orderReportByMonthAndShopState 
            = new ReportSectionState<List<GetOrderReportByShopAndByMonthQuery.DataOrderReportInDayDto>>();

        //void OnChange(DateTime? value)
        //{
        //    _selectedDate = value;
        //}

        private async Task OnViewRevenueByDateButtonClicked()
        {
            if (_selectedDate == null || _selectedShopIdDateSection == null) return;

            _orderReportByDateAndShopState.IsLoading = true;

            _orderReportByDateAndShopState.Details = await _mediator.Send(new GetOrderReportByShopAndByDateQuery
            {
                FromDate = StartOfDay(_selectedDate.GetValueOrDefault()),
                ToDate = EndOfDay(_selectedDate.GetValueOrDefault()),
                ShopId = _selectedShopIdDateSection.GetValueOrDefault()
            });

            _orderReportByDateAndShopState.IsReady = true;

            _orderReportByDateAndShopState.IsLoading = false;
        }

        private async Task OnViewRevenueByMonthButtonClicked()
        {
            if (_selectedMonth == null || _selectedShopIdMonthSection == null) return;

            _orderReportByMonthAndShopState.IsLoading = true;

            _orderReportByMonthAndShopState.Details = await _mediator.Send(new GetOrderReportByShopAndByMonthQuery
            {
                FromDate = StartOfMonth(_selectedMonth.GetValueOrDefault()),
                ToDate = EndOfMonth(_selectedMonth.GetValueOrDefault()),
                ShopId = _selectedShopIdMonthSection.GetValueOrDefault()
            });

            _orderReportByMonthAndShopState.IsReady = true;

            _orderReportByMonthAndShopState.IsLoading = false;
        }

        private DateTime StartOfDay(DateTime theDate)
        {
            return theDate.Date;
        }

        private DateTime EndOfDay(DateTime theDate)
        {
            return theDate.Date.AddDays(1).AddTicks(-1);
        }

        private DateTime StartOfMonth(DateTime theDate)
        {
            return new DateTime(theDate.Year, theDate.Month, 1);
        }

        private DateTime EndOfMonth(DateTime theDate)
        {
            return StartOfMonth(theDate).AddMonths(1).AddTicks(-1);
        }

        string FormatAsVND(object value)
        {
            return ((double)value).ToString("N0") + "đ";
        }

        public class ReportSectionState<T> where T : class
        {
            public T Details { get; set; } = default;
            public bool IsLoading { get; set; }

            public bool IsReady { get; set; }
        }
    }
}
