namespace Raci.Web.BlazorServer.Shared.Components
{
    using Microsoft.AspNetCore.Components;
    using Raci.Common.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class SelectEnumSingle<TEnum> where TEnum : struct
    {
        private string _selectedValue;

        private string SelectedValue
        {
            get
            {
                return _selectedValue;
            }

            set
            {
                _selectedValue = value;

                HandleOnChanged();
            }
        }

        [Parameter]
        public bool IsRequired { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public bool DisplayNotSet { get; set; } = true;

        [Parameter]
        public TEnum Value { get; set; }

        [Parameter]
        public EventCallback<TEnum> ValueChanged { get; set; }

        [Parameter]
        public RenderFragment<TEnum>? ItemTemplate { get; set; }

        [Parameter]
        public Func<IEnumerable<TEnum>> DataSource { get; set; }

        protected override void OnParametersSet()
        {
            _selectedValue = Value.ToEnumString();
        }

        private async void HandleOnChanged()
        {
            var enumValue = _selectedValue.ToEnum<TEnum>();

            await ValueChanged.InvokeAsync(enumValue).ConfigureAwait(false);
        }
    }
}
