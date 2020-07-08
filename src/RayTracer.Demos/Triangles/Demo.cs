using RayTracer.Demos.Logs;
using RayTracer.Lights;
using RayTracer.Matrices;
using RayTracer.Patterns;
using RayTracer.Shapes;
using RayTracer.Tuples;
using System;
using System.IO;

namespace RayTracer.Demos.Triangles
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 16;
        public static string Name { get; } = "Triangles";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            var world = new World();

            var lines = File.ReadLines("Triangles/Sonic.obj");

            var objParser = new ObjParser(lines);

            var sonic = objParser.ToGroup();
            var room = new Cube
            {
                Transform = Matrix.Translation(objParser.Middle.X, objParser.Middle.Y, objParser.Middle.Z)
                                   .Scale(100, 100, 100)
            };

            room.Material.Pattern = new CheckersPattern(Color.White, new Color(0.25, 0.25, 0.25))
            {
                Transform = Matrix.Scaling(0.1, 0.1, 0.1)
            };

            world.Objects.Add(sonic);
            world.Objects.Add(room);
            world.Lights.Add(new PointLight(new Point(10, 10, 10), new Color(1, 1, 1)));

            var camera = new Camera(width, height, Math.PI / 3);
            camera.Transform = Matrix.View(
                new Point(objParser.Middle.Y, objParser.Middle.Y, objParser.Maximum.Z + 50),
                objParser.Middle,
                new Vector(0, 1, 0));

            // Render the result to a canvas.
            var canvas = camera.Render(world);

            return canvas;
        }
    }
}
