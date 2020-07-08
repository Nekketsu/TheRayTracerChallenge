using RayTracer.Tuples;

namespace _00_ProjectileDemo
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
