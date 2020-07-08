using RayTracer.Shapes;
using RayTracer.Tuples;

namespace RayTracer.Intersections
{
    public class IntersectionWithUV : Intersection
    {
        public double U { get; }
        public double V { get; }

        public IntersectionWithUV(double t, Shape @object, double u, double v) : base(t, @object)
        {
            U = u;
            V = v;
        }

        protected override Vector NormalAt(Point point)
        {
            return Object.NormalAt(point, this);
        }
    }
}
