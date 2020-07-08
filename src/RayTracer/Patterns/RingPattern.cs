using RayTracer.Tuples;
using System;

namespace RayTracer.Patterns
{
    public class RingPattern : Pattern
    {
        public Color ColorA { get; }
        public Color ColorB { get; }

        public RingPattern(Color colorA, Color colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }

        public override Color PatternAt(Point point)
        {
            return Math.Floor(Math.Sqrt(point.X * point.X + point.Z * point.Z)) % 2 == 0
                ? ColorA
                : ColorB;
        }
    }
}
