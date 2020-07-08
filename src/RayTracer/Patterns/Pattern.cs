using RayTracer.Matrices;
using RayTracer.Shapes;
using RayTracer.Tuples;

namespace RayTracer.Patterns
{
    public abstract class Pattern
    {
        public Matrix Transform { get; set; }

        public Pattern()
        {
            Transform = Matrix.Identity;
        }

        public Color PatternAtShape(Shape shape, Point worldPoint)
        {
            var shapePoint = shape.WorldToObject(worldPoint);
            var patternPoint = (Point)(Transform.Inverse() * shapePoint);

            return PatternAt(patternPoint);
        }

        public abstract Color PatternAt(Point point);
    }
}
