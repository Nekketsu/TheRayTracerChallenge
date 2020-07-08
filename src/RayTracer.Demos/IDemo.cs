using RayTracer.Demos.Logs;

namespace RayTracer.Demos
{
    public interface IDemo
    {
        public static int Order { get; }
        public static string Name { get; }

        Canvas Run(int width = 100, int height = 50, ILogger logger = null);
    }
}
