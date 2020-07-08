using RayTracer.Demos.Logs;
using RayTracer.Lights;
using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;

namespace RayTracer.Demos.Groups
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 15;
        public static string Name { get; } = "Groups";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            var world = new World();

            const int hexagonCount = 2;

            for (var i = 0; i < hexagonCount; i++)
            {
                var hexagon = Hexagon();
                hexagon.Transform = Matrix.Translation(0, i, 0);

                world.Objects.Add(hexagon);
            }

            world.Lights.Add(new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)));

            var camera = new Camera(width, height, Math.PI / 3);
            camera.Transform = Matrix.View(
                new Point(0, 4, -5),
                new Point(0, 1, 0),
                new Vector(0, 1, 0));

            // Render the result to a canvas.
            var canvas = camera.Render(world);

            return canvas;
        }

        private Shape HexagonCorner()
        {
            var corner = new Sphere();
            corner.Transform = Matrix.Translation(0, 0, -1) * Matrix.Scaling(0.25, 0.25, 0.25);

            return corner;
        }

        private Shape HexagonEdge()
        {
            var edge = new Cylinder();
            edge.Minimum = 0;
            edge.Maximum = 1;
            edge.Transform = Matrix.Translation(0, 0, -1) *
                             Matrix.RotationY(-Math.PI / 6) *
                             Matrix.RotationZ(-Math.PI / 2) *
                             Matrix.Scaling(0.25, 1, 0.25);

            return edge;
        }

        private Shape HexagonSide()
        {
            var side = new Group();

            side.AddChild(HexagonCorner());
            side.AddChild(HexagonEdge());

            return side;
        }

        private Shape Hexagon()
        {
            var hexagon = new Group();

            for (var n = 0; n < 6; n++)
            {
                var side = HexagonSide();
                side.Transform = Matrix.RotationY(n * Math.PI / 3);
                hexagon.AddChild(side);
            }

            return hexagon;
        }
    }
}
