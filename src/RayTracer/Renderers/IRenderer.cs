using System.Threading.Tasks;

namespace RayTracer.Renderers
{
    public interface IRenderer
    {
        Task RenderAsync(Canvas canvas);
    }
}
