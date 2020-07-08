using RayTracer.Tuples;
using System;

namespace RayTracer.Patterns
{
    public class GradientPattern : Pattern
    {
        public Color ColorA { get; }
        public Color ColorB { get; }

        public GradientPattern(Color colorA, Color colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }

        public override Color PatternAt(Point point)
        {
            var distance = ColorB - ColorA;
            var fraction = point.X - Math.Floor(point.X);

            return ColorA + distance * fraction;
        }
    }
}
