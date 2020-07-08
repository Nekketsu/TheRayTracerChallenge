using RayTracer.Extensions;
using RayTracer.Intersections;
using RayTracer.Tuples;

namespace RayTracer.Shapes
{
    public class Plane : Shape
    {
        public Vector Normal { get; }

        public Plane()
        {
            Normal = new Vector(0, 1, 0);
        }

        public override IntersectionCollection LocalIntersect(Ray ray)
        {
            if (ray.Direction.Y.EqualsEpsilon(0))
            {
                return new IntersectionCollection();
            }

            var t = -ray.Origin.Y / ray.Direction.Y;

            return new IntersectionCollection(new Intersection(t, this));
        }

        public override Vector LocalNormalAt(Point point, IntersectionWithUV hit = null)
        {
            return Normal;
        }
    }
}
