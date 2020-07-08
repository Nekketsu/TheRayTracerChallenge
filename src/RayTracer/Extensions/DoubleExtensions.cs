using System;

namespace RayTracer.Extensions
{
    public static class DoubleExtensions
    {
        public const double Epsilon = 0.0001;

        public static bool EqualsEpsilon(this double a, double b)
        {
            return Math.Abs(a - b) < Epsilon;
        }

        public static byte To255Byte(this double value)
        {
            return (byte)Math.Round((255 * Math.Clamp(value, 0, 1)));
        }
    }
}
