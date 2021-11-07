using System;
using Microsoft.AspNetCore.Components;

namespace Raco.Blazor.DropzoneJs
{
    public class SectionTemplate : ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

    }
}
