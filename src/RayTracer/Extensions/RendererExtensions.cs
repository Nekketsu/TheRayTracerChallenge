using RayTracer.Renderers;
using System.Threading.Tasks;

namespace RayTracer.Extensions
{
    public static class RendererExtensions
    {
        public static async Task RenderAsync(this Canvas canvas, IRenderer renderer)
        {
            await renderer.RenderAsync(canvas);
        }
    }
}
