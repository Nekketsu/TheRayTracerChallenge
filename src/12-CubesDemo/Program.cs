using RayTracer.Demos.Cubes;
using RayTracer.Demos.Logs;
using RayTracer.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace _12_CubesDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var demo = new Demo();
            var canvas = demo.Run(logger: logger);

            var fileName = Demo.Name;

            using (var file = File.Create($"{fileName}.ppm"))
            {
                await canvas.ToPpmAsync(file);
                logger.WriteLine();
                logger.WriteLine($"Scene saved in \"{fileName}.ppm\"");
            }

            var image = canvas.ToImage();
            image.Save($"{fileName}.png");
            logger.WriteLine($"Scene saved in \"{fileName}.png\"");
        }
    }
}
