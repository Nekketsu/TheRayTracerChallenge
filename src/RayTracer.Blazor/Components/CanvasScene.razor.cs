using Microsoft.AspNetCore.Components;
using RayTracer.Blazor.Renderers;
using RayTracer.Extensions;
using System.Threading.Tasks;

namespace RayTracer.Blazor.Components
{
    public partial class CanvasScene
    {
        [Parameter]
        public Canvas Canvas { get; set; }

        public ElementReference CanvasElement { get; set; }

        Canvas previousCanvas;

        CanvasRenderer canvasRenderer;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                canvasRenderer = new CanvasRenderer(JSRuntime, CanvasElement);
            }

            if (Canvas != previousCanvas)
            {
                await Canvas.RenderAsync(canvasRenderer);

                previousCanvas = Canvas;
            }
        }
    }
}
