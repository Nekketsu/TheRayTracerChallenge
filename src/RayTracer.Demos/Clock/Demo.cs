using RayTracer.Demos.Logs;
using RayTracer.Matrices;
using RayTracer.Tuples;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Demos.Clock
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 2;
        public static string Name { get; } = "Clock";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            const int hours = 12;
            var clock = new List<Point>();

            var twelve = new Point(0, 0, 1);
            var step = 2 * Math.PI / hours;

            for (var hour = 0; hour < hours; hour++)
            {
                var rotationYTransform = Matrix.RotationY(hour * step);
                var hourMark = rotationYTransform * twelve;
                clock.Add((Point)hourMark);
            }

            var canvas = new Canvas(900, 550);
            var scaling = Math.Min(canvas.Width, canvas.Height) * 3 / 8;

            var transform = Matrix.Scaling(scaling, scaling, scaling)
                                  .Translate(canvas.Width / 2, 0, canvas.Height / 2);

            var transformedClock = clock.Select(h => (Point)(transform * h));

            var red = new Color(1, 0, 0);
            foreach (var hour in transformedClock)
            {
                var x = (int)Math.Round(hour.X);
                var y = canvas.Height - (int)Math.Round(hour.Z);

                canvas[x, canvas.Height - y] = red;
            }

            return canvas;
        }
    }
}
