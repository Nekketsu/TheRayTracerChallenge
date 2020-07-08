using RayTracer.Tuples;
using System;

namespace RayTracer.Patterns
{
    public class StripePattern : Pattern
    {
        public Color ColorA { get; }
        public Color ColorB { get; }

        public StripePattern(Color colorA, Color colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }

        public override Color PatternAt(Point point)
        {
            return Math.Floor(point.X) % 2 == 0 ? ColorA : ColorB;
        }
    }
}
