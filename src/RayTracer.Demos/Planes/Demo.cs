using RayTracer.Demos.Logs;
using RayTracer.Lights;
using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;

namespace RayTracer.Demos.Planes
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 7;
        public static string Name { get; } = "Planes";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            var floor = new Plane
            {
                Material = new Material
                {
                    Color = new Color(1, 0.9, 0.9),
                    Specular = 0
                }
            };

            var middle = new Sphere
            {
                Transform = Matrix.Translation(-0.5, 1, 0.5),
                Material = new Material
                {
                    Color = new Color(0.1, 1, 0.5),
                    Diffuse = 0.7,
                    Specular = 0.3
                }
            };

            var right = new Sphere
            {
                Transform = Matrix.Translation(1.5, 0.5, -0.5) *
                            Matrix.Scaling(0.5, 0.5, 0.5),
                Material = new Material
                {
                    Color = new Color(0.5, 1, 0.1),
                    Diffuse = 0.7,
                    Specular = 0.3
                }
            };

            var left = new Sphere
            {
                Transform = Matrix.Translation(-1.5, 0.33, -0.75) *
                            Matrix.Scaling(0.33, 0.33, 0.33),
                Material = new Material
                {
                    Color = new Color(1, 0.8, 0.1),
                    Diffuse = 0.7,
                    Specular = 0.3
                }
            };

            var world = new World();
            world.Lights.Add(new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)));
            world.Objects.AddRange(new Shape[] { floor, middle, right, left });

            var camera = new Camera(width, height, Math.PI / 3);
            camera.Transform = Matrix.View(
                new Point(0, 1.5, -5),
                new Point(0, 1, 0),
                new Vector(0, 1, 0));

            // Render the result to a canvas.
            var canvas = camera.Render(world);

            return canvas;
        }
    }
}
