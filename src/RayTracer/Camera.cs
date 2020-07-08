using RayTracer.Matrices;
using RayTracer.Tuples;
using System;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Camera
    {
        public int HorizontalSize { get; }
        public int VerticalSize { get; }
        public double FieldOfView { get; }

        public Matrix Transform { get; set; }

        public double PixelSize { get; }
        public double HalfWidth { get; }
        public double HalfHeight { get; set; }

        public Camera(int horizontalSize, int verticalSize, double fieldOfView)
        {
            HorizontalSize = horizontalSize;
            VerticalSize = verticalSize;
            FieldOfView = fieldOfView;

            Transform = Matrix.Identity;

            var halfView = Math.Tan(fieldOfView / 2);
            var aspect = (double)horizontalSize / verticalSize;

            if (aspect >= 1)
            {
                HalfWidth = halfView;
                HalfHeight = halfView / aspect;
            }
            else
            {
                HalfWidth = halfView * aspect;
                HalfHeight = halfView;
            }

            PixelSize = HalfWidth * 2 / horizontalSize;
        }

        public Ray RayForPixel(int px, int py)
        {
            // The offset from the edge of the canvas to the pixel's center
            var xOffset = (px + 0.5) * PixelSize;
            var yOffset = (py + 0.5) * PixelSize;

            // The unstranformed coordinates of the pixel in world space.
            // (Remember that the camera looks toward -z, so +x is to the *left*.)
            var worldX = HalfWidth - xOffset;
            var worldY = HalfHeight - yOffset;

            // Using the camera matrix, transform the canvas point and the origin,
            // and then compute the ray's director vector.
            // (Remember that the canvas is at z = -1)
            var inverseTransform = Transform.Inverse();
            var pixel = (Point)(inverseTransform * new Point(worldX, worldY, -1));
            var origin = (Point)(inverseTransform * new Point(0, 0, 0));
            var direction = (pixel - origin).Normalize();

            return new Ray(origin, direction);
        }

        public Canvas Render(World world, bool isParallel = false)
        {
            return isParallel ? RenderParallel(world) : RenderSequencial(world);
        }

        private Canvas RenderSequencial(World world)
        {
            var image = new Canvas(HorizontalSize, VerticalSize);

            for (var y = 0; y < VerticalSize; y++)
            {
                for (var x = 0; x < HorizontalSize; x++)
                {
                    var ray = RayForPixel(x, y);
                    var color = world.ColorAt(ray);
                    image[x, y] = color;
                }
                Console.WriteLine($"Row: {y}");
            }

            return image;
        }

        public Canvas RenderParallel(World world)
        {
            var image = new Canvas(HorizontalSize, VerticalSize);

            Parallel.For(0, VerticalSize, y =>
            {
                Parallel.For(0, HorizontalSize, x =>
                {
                    var ray = RayForPixel(x, y);
                    var color = world.ColorAt(ray);
                    image[x, y] = color;
                });
                Console.WriteLine($"Row: {y}");
            });

            return image;
        }
    }
}
