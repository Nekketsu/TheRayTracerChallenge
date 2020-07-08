using RayTracer.Demos.Logs;
using RayTracer.Tuples;

namespace RayTracer.Demos.Spheres
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 3;
        public static string Name { get; } = "Spheres";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            // Start the ray at z = -5
            var rayOrigin = new Point(0, 0, -5);

            // Put the wall at z = 10
            var wallZ = 10;

            var wallSize = 7.0;
            var canvasPixels = 100;
            var pixelSize = wallSize / canvasPixels;
            var half = wallSize / 2;

            var canvas = new Canvas(canvasPixels, canvasPixels);
            var color = new Color(1, 0, 0); // Red
            var shape = new Shapes.Sphere();

            //// Shrink it along the y axis
            //shape.Transform = Matrix.Scaling(1, 0.5, 1);

            //// Shrink it along the x axis
            //shape.Transform = Matrix.Scaling(0.5, 1, 1);

            //// Shrink it, and rotate it!
            //shape.Transform = Matrix.RotationZ(Math.PI / 4) * Matrix.Scaling(0.5, 1, 1);

            //// Shrink it, and skew it!
            //shape.Transform = Matrix.Shearing(1, 0, 0, 0, 0, 0) * Matrix.Scaling(0.5, 1, 1);

            // For each row of pixels in the canvas
            for (var y = 0; y < canvasPixels; y++)
            {
                // Compute the world y coordinate (top = +half, bottom = -half)
                var worldY = half - pixelSize * y;

                // For each pixel in the row
                for (var x = 0; x < canvasPixels; x++)
                {
                    // Compute the world x coordnate (left = -half, right = half)
                    var worldX = -half + pixelSize * x;

                    // Describe the point on the wall that the ray will target
                    var position = new Point(worldX, worldY, wallZ);

                    var r = new Ray(rayOrigin, (position - rayOrigin).Normalize());
                    var xs = shape.Intersect(r);

                    if (xs.Hit != null)
                    {
                        canvas[x, y] = color;
                    }
                }
            }

            return canvas;
        }
    }
}
