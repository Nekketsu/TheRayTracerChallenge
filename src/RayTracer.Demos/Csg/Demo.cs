using RayTracer.Demos.Logs;
using RayTracer.Lights;
using RayTracer.Materials;
using RayTracer.Matrices;
using RayTracer.Patterns;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;

namespace RayTracer.Demos.Csg
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 17;
        public static string Name { get; } = "CSG";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            var world = new World();

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

            var shape = CreateShape();
            shape.Transform = shape.Transform.Translate(0, 2, 0);

            world.Lights.Add(new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)));
            world.Objects.AddRange(new Shape[] { shape });

            var camera = new Camera(width, height, Math.PI / 3);
            camera.Transform = Matrix.View(
                new Point(-3, 5, -5),
                new Point(0, 2, 0),
                new Vector(0, 1, 0));

            // Render the result to a canvas.
            var canvas = camera.Render(world);

            return canvas;
        }

        private Cylinder CreateCylinder(Matrix transform)
        {
            var aspect = 3.0;
            return new Cylinder
            {
                Minimum = -(3 * aspect),
                Maximum = 3 * aspect,
                Closed = true,
                Transform = transform.Scale(1 / aspect, 1 / aspect, 1 / aspect)
            };
        }

        private Shape CreateCylinders()
        {
            return CreateCylinder(Matrix.Identity) +
                   CreateCylinder(Matrix.RotationX(Math.PI / 2)) +
                   CreateCylinder(Matrix.RotationX(Math.PI / 2).RotateY(Math.PI / 2));
        }

        private Sphere CreateSphere()
        {
            var scale = Math.Sqrt(2);

            return new Sphere
            {
                Transform = Matrix.Scaling(scale, scale, scale)
            };
        }

        private Cube CreateCube()
        {
            return new Cube();
        }

        private Shape CreateBox()
        {
            return CreateSphere() *
                   CreateCube();
        }

        private Shape CreateShape()
        {
            return CreateBox() -
                   CreateCylinders();
        }
    }
}
