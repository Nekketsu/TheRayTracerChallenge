using RayTracer.Demos.Logs;
using RayTracer.Lights;
using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Patterns;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;

namespace RayTracer.Demos.Cones
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 14;
        public static string Name { get; } = "Cones";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            var room = new Cube
            {
                Transform = Matrix.Translation(0, 1, 0).Scale(20, 20, 20),
                Material = new Material
                {
                    Pattern = new CheckersPattern(new Color(0.8, 0.8, 0.8), new Color(0.2, 0.2, 0.2))
                    {
                        Transform = Matrix.Scaling(0.1, 0.1, 0.1)
                    },
                    Specular = 0
                }
            };

            var woodColor = new Color(0.52, 0.37, 0.26);
            var tableLeg1 = new Cube
            {
                Transform = Matrix.Translation(0, 1, 0).Scale(0.5, 2.5, 0.5).Translate(2.5, 0, 2.5),
                Material = new Material
                {
                    Color = woodColor,
                    Specular = 0
                }
            };

            var tableLeg2 = new Cube
            {
                Transform = Matrix.Translation(0, 1, 0).Scale(0.5, 2.5, 0.5).Translate(2.5, 0, -2.5),
                Material = new Material
                {
                    Color = woodColor,
                    Specular = 0
                }
            };

            var tableLeg3 = new Cube
            {
                Transform = Matrix.Translation(0, 1, 0).Scale(0.5, 2.5, 0.5).Translate(-2.5, 0, -2.5),
                Material = new Material
                {
                    Color = woodColor,
                    Specular = 0
                }
            };

            var tableLeg4 = new Cube
            {
                Transform = Matrix.Translation(0, 1, 0).Scale(0.5, 2.5, 0.5).Translate(-2.5, 0, 2.5),
                Material = new Material
                {
                    Color = woodColor,
                    Specular = 0
                }
            };

            var table = new Cube
            {
                Transform = Matrix.Translation(0, 1, 0).Scale(5, 0.5, 5).Translate(0, 5, 0),
                Material = new Material
                {
                    Color = woodColor,
                    Specular = 0
                }
            };

            var cone = new Cone
            {
                Minimum = 0,
                Maximum = 1.5,
                Closed = true,
                Transform = Matrix.Scaling(2, 2, 2).Translate(-2, 5.5, -0.5),
                Material = new Material
                {
                    Color = new Color(0.8, 0.2, 0.8),
                    Specular = 1,
                    Shininess = 300,
                    Transparency = 1,
                    Reflective = 0.5,
                    RefractiveIndex = 0.2,
                    Diffuse = 0.5
                }
            };


            var world = new World();
            world.Lights.Add(new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)));
            world.Objects.AddRange(new Shape[] { room, tableLeg1, tableLeg2, tableLeg3, tableLeg4, table, cone });

            var camera = new Camera(width, height, Math.PI / 3);
            camera.Transform = Matrix.View(
                new Point(7.5, 10, -15),
                new Point(0, 5, 0),
                new Vector(0, 1, 0));

            // Render the result to a canvas.
            var canvas = camera.Render(world);

            return canvas;
        }
    }
}
