using RayTracer.Tuples;

namespace RayTracer.Demos.Projectile
{
    public class Environment
    {
        public Vector Gravity { get; }
        public Vector Wind { get; }

        public Environment(Vector gravity, Vector wind)
        {
            Gravity = gravity;
            Wind = wind;
        }
    }
}
