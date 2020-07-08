using RayTracer.Patterns;
using RayTracer.Tuples;

namespace RayTracer.Tests.Entities
{
    public class TestPattern : Pattern
    {
        public override Color PatternAt(Point point)
        {
            return new Color(point.X, point.Y, point.Z);
        }
    }
}
