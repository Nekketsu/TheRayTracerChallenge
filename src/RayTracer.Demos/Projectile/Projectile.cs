using RayTracer.Tuples;

namespace RayTracer.Demos.Projectile
{
    public class Projectile
    {
        public Point Position { get; }
        public Vector Velocity { get; }

        public Projectile(Point position, Vector velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }
}
