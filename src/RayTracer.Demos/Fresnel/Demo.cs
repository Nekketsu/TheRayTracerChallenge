using RayTracer.Demos.Logs;
using RayTracer.Lights;
using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Patterns;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;

namespace RayTracer.Demos.Fresnel
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 11;
        public static string Name { get; } = "Fresnel";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            var floor = new Plane
            {
                Material = new Material
                {
                    Pattern = new CheckersPattern(new Color(0.8, 0.8, 0.8), new Color(0.2, 0.2, 0.2)),
                    Specular = 0
                }
            };

            var water = new Plane
            {
                Transform = Matrix.Translation(0, 0.5, 0),
                Material = new Material
                {
                    Color = new Color(0.1, 0.1, 0.2),
                    Specular = 1,
                    Shininess = 300,
                    Transparency = 0.9,
                    Reflective = 0.9
                }
            };

            var horizont = new Plane
            {
                Transform = Matrix.RotationX(Math.PI / 2).Translate(0, 0, 5),
                Material = new Material
                {
                    Pattern = new CheckersPattern(new Color(0.9, 0.9, 1), Color.White),
                    Specular = 0
                }
            };

            var sun = new Sphere
            {
                Transform = Matrix.Scaling(0.4, 0.4, 0.4).Translate(1, 2, 0),
                Material = new Material
                {
                    Color = new Color(0.9, 0.8, 0.3)
                }
            };

            var middle = new GlassSphere
            {
                Transform = Matrix.Translation(-0.5, 1, 0.5)
            };

            middle.Material.Color = new Color(0.15, 0.15, 0.7);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            middle.Material.Reflective = 1;

            var right = new Sphere
            {
                Transform = Matrix.Translation(1.5, 0.5, -0.5) *
                            Matrix.Scaling(0.5, 0.5, 0.5),
                Material = new Material
                {
                    Color = new Color(0.15, 0.7, 0.15),
                    Diffuse = 0.7,
                    Specular = 0.3,
                    Reflective = 0.67
                }
            };

            var left = new Sphere
            {
                Transform = Matrix.Translation(-1.5, 0.33, -0.75) *
                            Matrix.Scaling(0.33, 0.33, 0.33),
                Material = new Material
                {
                    Color = new Color(0.7, 0.15, 0.15),
                    Diffuse = 0.7,
                    Specular = 0.3,
                    Reflective = 0.33
                }
            };

            var world = new World();
            world.Lights.Add(new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)));
            world.Objects.AddRange(new Shape[] { floor, water, horizont, sun, middle, right, left });

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
