using RayTracer.Demos.Logs;
using RayTracer.Lights;
using RayTracer.Tuples;

namespace RayTracer.Demos.Spheres3D
{
    public class Demo : IDemo
    {
        public static int Order { get; } = 4;
        public static string Name { get; } = "Spheres3D";

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
            var sphere = new Shapes.Sphere();
            sphere.Material.Color = new Color(1, 0.2, 1);

            var lightPosition = new Point(-10, 10, -10);
            var lightColor = new Color(1, 1, 1);
            var light = new PointLight(lightPosition, lightColor);

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
                    var xs = sphere.Intersect(r);

                    if (xs.Hit != null)
                    {
                        var point = r.Position(xs.Hit.T);
                        var normal = xs.Hit.Object.NormalAt(point);
                        var eye = -r.Direction;

                        var color = xs.Hit.Object.Material.Lighting(sphere, light, point, eye, normal);

                        canvas[x, y] = color;
                    }
                }
            }

            return canvas;
        }
    }
}
