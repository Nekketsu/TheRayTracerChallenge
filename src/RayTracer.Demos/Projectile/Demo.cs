using RayTracer.Demos.Logs;
using RayTracer.Tuples;
using System;

namespace RayTracer.Demos.Projectile
{
    public class Demo : IDemo
    {
        public static int Order => 2;
        public static string Name => "Projectile";

        public Canvas Run(int width = 100, int height = 50, ILogger logger = null)
        {
            var start = new Point(0, 1, 0);
            var velocity = new Vector(1, 1.8, 0).Normalize() * 11.25;
            var projectile = new Projectile(start, velocity);

            var gravity = new Vector(0, -0.1, 0);
            var wind = new Vector(-0.01, 0, 0);
            var environment = new Environment(gravity, wind);

            var canvas = new Canvas(900, 550);

            logger?.WriteLine($"Projectile: {projectile.Position}");
            DrawProjectile(canvas, projectile);
            while (projectile.Position.Y > 0)
            {
                projectile = Tick(environment, projectile);
                logger?.WriteLine($"Projectile: {projectile.Position}");
                DrawProjectile(canvas, projectile);
            }

            return canvas;
        }

        private Projectile Tick(Environment environment, Projectile projectile)
        {
            var position = projectile.Position + projectile.Velocity;
            var velocity = projectile.Velocity + environment.Gravity + environment.Wind;

            return new Projectile(position, velocity);
        }

        private void DrawProjectile(Canvas canvas, Projectile projectile)
        {
            var x = (int)Math.Round(projectile.Position.X);
            var y = canvas.Height - (int)Math.Round(projectile.Position.Y);
            var red = new Color(1, 0, 0);
            canvas[x, y] = red;
        }
    }
}
