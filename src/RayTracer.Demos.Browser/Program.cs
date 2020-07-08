using RayTracer.Demos.Logs;
using RayTracer.Demos.Services;
using RayTracer.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RayTracer.Demos.Browser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var program = new Program();
            await program.RunAsync();
        }

        ILogger logger;
        DemoService demoService;

        public Program()
        {
            logger = new ConsoleLogger();
            demoService = new DemoService();
        }

        public async Task RunAsync()
        {
            var demos = demoService.GetDemos().ToArray();

            string line;
            do
            {
                var i = 1;
                foreach (var demo in demos)
                {
                    logger.WriteLine($"{i++}: {demo}");
                }
                logger.WriteLine("0: exit");
                logger.Write("> ");
                line = Console.ReadLine();
                if (int.TryParse(line, out var option) &&
                    option >= 1 && option <= demos.Length)
                {
                    var demoName = demos[option - 1];
                    var demo = demoService.CreateDemo(demoName);
                    var canvas = demo.Run(logger: logger);

                    await SaveAsync(canvas, demoName);
                }
                else
                {
                    logger.WriteLine($"Invalid option \"{line}\"");
                }

                logger.WriteLine();
            } while (line != "0");
        }

        private async Task SaveAsync(Canvas canvas, string fileName)
        {
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
