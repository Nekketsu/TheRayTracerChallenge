using Microsoft.AspNetCore.Components;
using RayTracer.Extensions;

namespace RayTracer.Blazor.Components
{
    public partial class ImgScene
    {
        [Parameter]
        public Canvas Canvas { get; set; }

        public string Base64Image { get; set; }

        Canvas previousCanvas;

        protected override void OnAfterRender(bool firstRender)
        {
            if (Canvas != previousCanvas)
            {
                Base64Image = Canvas.ToBase64Image();

                previousCanvas = Canvas;

                StateHasChanged();
            }
        }
    }
}
