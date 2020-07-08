using RayTracer.Tuples;
using System;

namespace _00_ProjectileDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Projectile starts one unit above the origin.
            // Velocity is normalized to 1 unit/tick
            var projectile = new Projectile(new Point(0, 1, 0), new Vector(1, 1, 0).Normalize());

            // Gravity is -0.1 unit/tick, and wind is -0.01 unit/tick.
            var environment = new Environment(new Vector(0, -0.1, 0), new Vector(-0.01, 0, 0));

            Console.WriteLine($"Projectile: {projectile.Position}");
            while (projectile.Position.Y > 0)
            {
                projectile = Tick(environment, projectile);
                Console.WriteLine($"Projectile: {projectile.Position}");
            }
        }

        private static Projectile Tick(Environment environment, Projectile projectile)
        {
            var position = projectile.Position + projectile.Velocity;
            var velocity = projectile.Velocity + environment.Gravity + environment.Wind;

            return new Projectile(position, velocity);
        }
    }
}
