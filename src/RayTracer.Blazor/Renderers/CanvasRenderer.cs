using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RayTracer.Extensions;
using RayTracer.Renderers;
using System.Linq;
using System.Threading.Tasks;

namespace RayTracer.Blazor.Renderers
{
    public class CanvasRenderer : IRenderer
    {
        public IJSRuntime JSRuntime { get; }
        public ElementReference CanvasElement { get; }

        public CanvasRenderer(IJSRuntime jsRuntime, ElementReference canvasElement)
        {
            JSRuntime = jsRuntime;
            CanvasElement = canvasElement;
        }

        public async Task RenderAsync(Canvas canvas)
        {
            var data = canvas.Select(c => new { Red = c.Red.To255Byte(), Green = c.Green.To255Byte(), Blue = c.Blue.To255Byte() });
            await JSRuntime.InvokeVoidAsync("canvas.render", CanvasElement, canvas.Width, canvas.Height, data);
        }

        //public async Task RenderAsync(Canvas canvas)
        //{
        //    await JSRuntime.InvokeVoidAsync("canvas.setSize", CanvasElement, canvas.Width, canvas.Height);
        //    await JSRuntime.InvokeVoidAsync("canvas.startDrawing", CanvasElement);

        //    var i = 0;
        //    foreach (var color in canvas)
        //    {
        //        var htmlColor = new { Red = color.Red.To255Byte(), Green = color.Green.To255Byte(), Blue = color.Blue.To255Byte() };
        //        await JSRuntime.InvokeVoidAsync("canvas.setColor", i++, color.Red.To255Byte());
        //    }

        //    await JSRuntime.InvokeVoidAsync("canvas.endDrawing");
        //}
    }
}
