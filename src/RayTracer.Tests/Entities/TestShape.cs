using RayTracer.Intersections;
using RayTracer.Shapes;
using RayTracer.Tuples;

namespace RayTracer.Tests.Entities
{
    public class TestShape : Shape
    {
        public Ray SavedRay { get; private set; }

        public override IntersectionCollection LocalIntersect(Ray ray)
        {
            SavedRay = ray;

            return null;
        }

        public override Vector LocalNormalAt(Point point, IntersectionWithUV hit = null)
        {
            return new Vector(point.X, point.Y, point.Z);
        }
    }
}
