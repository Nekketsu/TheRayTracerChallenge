using RayTracer.Demos.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RayTracer.Blazor.Pages
{
    public partial class RayTracerDemos
    {
        public IEnumerable<string> Demos { get; set; }
        public string SelectedDemo { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Canvas Canvas { get; set; }

        public bool IsLoading { get; set; }

        DemoService demoService;

        public RayTracerDemos()
        {
            demoService = new DemoService();

            Demos = demoService.GetDemos();
            SelectedDemo = Demos.FirstOrDefault(d => d == RayTracer.Demos.Shadows.Demo.Name);

            Width = 100;
            Height = 50;

            IsLoading = false;
        }

        public async void RenderDemo()
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1); // Trick to flush the changes (doesn't just work with StateHasChanged())

            var demo = demoService.CreateDemo(SelectedDemo);
            Canvas = demo.Run(Width, Height);

            IsLoading = false;
            StateHasChanged();
        }
    }
}
