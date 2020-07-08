using RayTracer.Tuples;
using System;

namespace RayTracer.Patterns
{
    public class CheckersPattern : Pattern
    {
        public Color ColorA { get; }
        public Color ColorB { get; }

        public CheckersPattern(Color colorA, Color colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }

        public override Color PatternAt(Point point)
        {
            return (Math.Floor(point.X) + Math.Floor(point.Y) + Math.Floor(point.Z)) % 2 == 0
                ? ColorA
                : ColorB;
        }
    }
}
