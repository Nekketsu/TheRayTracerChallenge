using RayTracer.Extensions;
using System;

namespace RayTracer
{
    public class Color
    {
        public static Color Black = new Color(0, 0, 0);
        public static Color White = new Color(1, 1, 1);

        public double Red { get; }
        public double Green { get; }
        public double Blue { get; }

        public Color(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public override bool Equals(object obj)
        {
            if (obj is Color color)
            {
                return color.Red.EqualsEpsilon(Red) &&
                       color.Green.EqualsEpsilon(Green) &&
                       color.Blue.EqualsEpsilon(Blue);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Red, Green, Blue);
        }

        public static bool operator ==(Color c1, Color c2)
        {
            return c1.Equals(c2);
        }
        public static bool operator !=(Color c1, Color c2)
        {
            return !c1.Equals(c2);
        }


        public Color Add(Color c)
        {
            return new Color(Red + c.Red, Green + c.Green, Blue + c.Blue);
        }

        public Color Substract(Color c)
        {
            return new Color(Red - c.Red, Green - c.Green, Blue - c.Blue);
        }

        public Color Multiply(double scalar)
        {
            return new Color(Red * scalar, Green * scalar, Blue * scalar);
        }

        public Color Multiply(Color c)
        {
            return new Color(Red * c.Red, Green * c.Green, Blue * c.Blue);
        }


        public static Color operator +(Color c1, Color c2)
        {
            return c1.Add(c2);
        }

        public static Color operator -(Color c1, Color c2)
        {
            return c1.Substract(c2);
        }

        public static Color operator *(Color c, double scalar)
        {
            return c.Multiply(scalar);
        }

        public static Color operator *(double scalar, Color c)
        {
            return c.Multiply(scalar);
        }

        public static Color operator *(Color c1, Color c2)
        {
            return c1.Multiply(c2);
        }
    }
}
